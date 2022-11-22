using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.API.Services;
using Microsoft.Extensions.Logging;
using DebtManagement.API.Grpc.Clients.StaticResource;
using AutoMapper;

namespace DebtManagement.API.Application.Commands.ReceiptVoucherCommandHandler
{
    public class
        CreateReceiptVoucherCommandHandler : IRequestHandler<CreateReceiptVoucherCommand,
            ActionResponse<ReceiptVoucherDTO>>
    {
        private readonly ILogger<CreateReceiptVoucherCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IReceiptVoucherService _receiptVoucherService;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IVoucherTargetQueries _voucherTargetQueries;
        private readonly IFeedbackGrpcService _feedbackService;
        private const int TRY_GENERATE_NEW_VOUCHER_CODE_TIMES = 50;
        private readonly IConfigurationSystemParameterGrpcService _configurationSystemParameterService;
        private readonly IExchangeRateGrpcService _exchangeRateGrpcService;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _fileRepository;
        private readonly IOutContractService _outContractService;
        // private readonly INotificationGrpcService _notificationGrpcService;

        public CreateReceiptVoucherCommandHandler(
            ILogger<CreateReceiptVoucherCommandHandler> logger,
            IMapper mapper,
            IMediator mediator,
            IReceiptVoucherService receiptVoucherService,
            IReceiptVoucherRepository receiptVoucherRepository,
            IVoucherTargetQueries voucherTargetQueries,
            IFeedbackGrpcService feedbackService,
            IConfigurationSystemParameterGrpcService configurationSystemParameterService,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IAttachmentFileRepository fileRepository,
            IExchangeRateGrpcService exchangeRateGrpcService,
            IReceiptVoucherQueries receiptVoucherQueries,
            IOutContractService outContractService)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _receiptVoucherService = receiptVoucherService;
            _receiptVoucherRepository = receiptVoucherRepository;
            _voucherTargetQueries = voucherTargetQueries;
            _feedbackService = feedbackService;
            _configurationSystemParameterService = configurationSystemParameterService;
            _exchangeRateGrpcService = exchangeRateGrpcService;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
            this._receiptVoucherQueries = receiptVoucherQueries;
            _outContractService = outContractService;
        }

        /// <summary>
        /// Xử lý thêm mới phiếu thu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<ReceiptVoucherDTO>> Handle(CreateReceiptVoucherCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var commandResponse = new ActionResponse<ReceiptVoucherDTO>();
                var systemConfiguration = await _configurationSystemParameterService.GetSystemConfigurations();

                #region Validate voucher code

                if (string.IsNullOrWhiteSpace(request.VoucherCode))
                {
                    request.VoucherCode = await this.GetVoucherCode(request.IssuedDate,
                        request.ProjectId,
                        request.MarketAreaId,
                        request.Target.IsEnterprise);
                }

                if (_receiptVoucherRepository.IsExistByCode(request.VoucherCode))
                {
                    commandResponse.AddError("Số phiếu thu đã tồn tại ", nameof(request.VoucherCode));
                    return commandResponse;
                }

                #endregion

                #region Binding initialization properties

                // Khởi tạo thể hiện của phiếu thu
                var voucherEntity = new ReceiptVoucher(request);
                if (!_voucherTargetQueries.IsExisted(request.Target.IdentityGuid))
                {
                    // Tạo mới/cập nhật contractor
                    request.Target.IsPayer = true;
                    request.Target.Id = 0;
                    var createTargetRsp = await _mediator.Send(request.Target, cancellationToken);
                    if (createTargetRsp.IsSuccess)
                    {
                        voucherEntity.SetTarget(createTargetRsp.Result.Id);
                    }
                    else
                    {
                        throw new DebtDomainException(createTargetRsp.Message);
                    }
                }
                else
                {
                    var selectedTarget = _voucherTargetQueries.Find(request.Target.IdentityGuid);
                    voucherEntity.SetTarget(selectedTarget.Id);
                }

                #endregion

                // Thêm danh sách chi tiết phiếu thu 
                if (request.ReceiptLines != null && request.ReceiptLines.Count > 0)
                {
                    request.ReceiptLines.ForEach(e =>
                    {
                        e.IdentityGuid = string.IsNullOrEmpty(e.IdentityGuid) ? Guid.NewGuid().ToString() : e.IdentityGuid;
                        voucherEntity.AddReceiptVoucherDetail(e);
                    });
                }

                // Thêm mới chi tiết thanh toán công nợ đầu kỳ
                if (request.OpeningDebtPayments != null
                    && request.OpeningDebtPayments.Any())
                {
                    request.OpeningDebtPayments.ForEach(voucherEntity.AddDebtPaymentDetail);
                }

                if (request.IncurredDebtPayments != null && request.IncurredDebtPayments.Count > 0)
                {
                    request.IncurredDebtPayments.ForEach(voucherEntity.UpdatePaymentDetail);
                }

                #region promotion

                if (request.PromotionForReceiptNews.Count > 0)
                {
                    foreach (var item in request.PromotionForReceiptNews)
                    {
                        var promo = _mapper.Map<PromotionForReceiptVoucher>(item);
                        promo.CreatedDate = DateTime.Now;
                        voucherEntity.SetPromotionForReceiptVoucher(promo);
                    }
                }

                #endregion

                if (request.CurrencyUnitCode == "VND")
                {
                    voucherEntity.ExchangeRate = 1;
                }
                else
                {
                    var erResult = await _exchangeRateGrpcService.ExchangeRate(request.CurrencyUnitCode, "VND");
                    if (erResult.IsSuccess) voucherEntity.ExchangeRate = erResult.Value;
                }

                voucherEntity.SetStatusId(ReceiptVoucherStatus.Pending.Id);
                voucherEntity.UpdateStatusOverdue();
                voucherEntity.UpdateStatusBadDebt(systemConfiguration.NumberDaysOverdue ?? 60);

                voucherEntity.ExchangeRateApplyDate = DateTime.Now;
                voucherEntity.CalculateTotal();

                var savedVoucherEntityRsp = await _receiptVoucherRepository.CreateAndSave(voucherEntity);
                commandResponse.CombineResponse(savedVoucherEntityRsp);
                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }
                else
                {
                    var resolvedFeedbacks = string.Join("##", request.ReceiptLines.SelectMany(r => r.ReductionDetails)
                        .Select(r => r.Id));
                    if (!string.IsNullOrEmpty(resolvedFeedbacks))
                    {
                        var markFeedbackResponse = await _feedbackService.MarkFeedbacksAsResolved(resolvedFeedbacks);
                        if (!markFeedbackResponse)
                            throw new DebtDomainException($"The following issues {resolvedFeedbacks} can not be marked as resolved");
                        foreach (var item in voucherEntity.ReceiptVoucherDetails)
                        {
                            await _feedbackService.UpdateReceiptLine(string.Join("##", item.ReceiptVoucherDetailReductions.Select(r => r.ReasonId)), item.Id);
                        }
                    }

                    //if (!string.IsNullOrEmpty(request.AccountingCode))
                    //{
                    //    var iType = ReceiptVoucherType.Signed.Id;
                    //    if (voucherEntity.IsFirstVoucherOfContract)
                    //    {

                    //    }
                    //    var notiReq = new PushNotificationRequest()
                    //    {
                    //        Zone = NotificationZone.Debt,
                    //        Type = NotificationType.BillingAlert,
                    //        Category = NotificationCategory.ReceiptVoucher,
                    //        Title = $"Tạo mới phiếu thu số {commandResponse.Result.VoucherCode}",
                    //        Content = $"Phiếu thu {commandResponse.Result.VoucherCode} cho hợp đồng số {commandResponse.Result.ContractCode}.",
                    //        Payload = JsonConvert.SerializeObject(new
                    //        {
                    //            Type = iType,
                    //            TypeName = ReceiptVoucherType.GetTypeName(iType),
                    //            Id = commandResponse.Result.Id,
                    //            Code = commandResponse.Result.VoucherCode,
                    //            Category = NotificationCategory.ReceiptVoucher
                    //        }, new JsonSerializerSettings
                    //        {
                    //            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //        })
                    //    };
                    //   // await _notificationGrpcService.PushNotificationByUids(notiReq, string.Join(',', commandResponse.Result.AccountingCode));
                    //}
                }

                var receiptVoucher = savedVoucherEntityRsp.Result;
                for (int i = 0; i < request.ReceiptLines.Count; i++)
                {
                    if (request.ReceiptLines[i].AttachmentFiles?.Any() ?? false)
                    {
                        var attachmentFiles =
                            await _attachmentFileService.PersistentFiles(request.ReceiptLines[i].AttachmentFiles.Select(c => c.TemporaryUrl)
                            .ToArray());
                        if (attachmentFiles == null || !attachmentFiles.Any())
                            throw new DebtDomainException("An error has occured when save the attachments");

                        foreach (var fileCommand in attachmentFiles)
                        {
                            fileCommand.CreatedBy = request.ReceiptLines[i].CreatedBy;
                            fileCommand.ReceiptVoucherDetailId
                                = receiptVoucher.ReceiptVoucherDetails
                                    .First(r => r.IdentityGuid == request.ReceiptLines[i].IdentityGuid).Id;
                            fileCommand.ReceiptVoucherId = receiptVoucher.Id;
                            var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                            if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                        }
                    }
                }

                if (request.ReceiptLines.Any(c => !string.IsNullOrEmpty(c.SPSuspensionTimeIds)))
                {
                    var suspensionTimeHandleds = string.Join(",", request.ReceiptLines
                        .Where(c => !string.IsNullOrEmpty(c.SPSuspensionTimeIds))
                        .Select(c => c.SPSuspensionTimeIds));
                    _outContractService.ActivateSuspensionHandled(suspensionTimeHandleds);
                }

                //if (ReceiptVoucherStatus.IsPaidStatus(voucherEntity.StatusId))
                //{
                //    _receiptVoucherRepository.UpdateApplicationUserClass(request.TargetId, request.OutContractId);
                //}

                return commandResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("---- Error handling CreateReceiptVoucherCommand command: {Exception}", ex);
                throw;
            }
        }

        private async Task<string> GetVoucherCode(DateTime issuedDate, int? projectId, int? marketAreaId, bool isEnterprise = false, int? vchrIndex = null, int tryTimes = 1)
        {

            var generatedCode = await _receiptVoucherService
                        .GenerateReceiptVoucherCode(issuedDate, projectId, marketAreaId, isEnterprise, vchrIndex);

            if (_receiptVoucherRepository.IsExistByCode(generatedCode))
            {
                if (tryTimes > TRY_GENERATE_NEW_VOUCHER_CODE_TIMES)
                {
                    return $"{generatedCode}/{issuedDate.Ticks}";
                }

                if (!vchrIndex.HasValue)
                {
                    vchrIndex = _receiptVoucherQueries.GetOrderNumberByDate(issuedDate);
                }
                return await GetVoucherCode(issuedDate, projectId, marketAreaId, isEnterprise, vchrIndex + 1, tryTimes + 1);
            }

            return generatedCode;
        }
    }
}