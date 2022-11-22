using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;

namespace DebtManagement.API.Application.Commands.PaymentVoucherCommandHandler
{
    public class CreatePaymentVoucherCommandHandler : IRequestHandler<CreatePaymentVoucherCommand, ActionResponse<PaymentVoucherDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;
        private readonly IVoucherTargetQueries _voucherTargetQueries;
        private readonly IExchangeRateGrpcService _exchangeRateGrpcService;
        private readonly IReceiptVoucherInPaymentVoucherRepository _reciptVoucherInPaymentVoucherRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _fileRepository;


        public CreatePaymentVoucherCommandHandler(
            IMediator mediator,
            IPaymentVoucherRepository paymentVoucherRepository,
            IVoucherTargetQueries voucherTargetQueries,
            IExchangeRateGrpcService exchangeRateGrpcService,
            IReceiptVoucherInPaymentVoucherRepository reciptVoucherInPaymentVoucherRepository,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IAttachmentFileRepository fileRepository)
        {
            _mediator = mediator;
            _paymentVoucherRepository = paymentVoucherRepository;
            _voucherTargetQueries = voucherTargetQueries;
            _exchangeRateGrpcService = exchangeRateGrpcService;
            _reciptVoucherInPaymentVoucherRepository = reciptVoucherInPaymentVoucherRepository;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Xử lý thêm mới phiếu chi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<PaymentVoucherDTO>> Handle(CreatePaymentVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<PaymentVoucherDTO>();
            if (_paymentVoucherRepository.IsExistByCode(request.VoucherCode))
            {
                commandResponse.AddError("Số phiếu đề nghị thanh toán đã tồn tại ", nameof(request.VoucherCode));
                return commandResponse;
            }

            #region Binding initialization properties

            // Khởi tạo thể hiện của phiếu chi
            var voucherEntity = new PaymentVoucher(request);

            #endregion

            if (!_voucherTargetQueries.IsExisted(request.Target.IdentityGuid))
            {
                // Tạo mới/cập nhật contractor
                request.Target.IsPayer = false;
                request.Target.IsPartner = true;
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
                var target = _voucherTargetQueries.Find(request.Target.IdentityGuid);
                voucherEntity.SetTarget(target.Id);
            }

            // Thêm danh sách chi tiết phiếu chi 
            if (request.PaymentVoucherDetails != null)
            {
                foreach (var pvd in request.PaymentVoucherDetails)
                {
                    pvd.IdentityGuid = string.IsNullOrEmpty(pvd.IdentityGuid) ? Guid.NewGuid().ToString() : pvd.IdentityGuid;
                    voucherEntity.AddPaymentVoucherDetail(pvd);
                }
            }

            // Thêm danh mục thuế áp dụng vào phiếu thu
            if (request.PaymentVoucherTaxes != null && request.PaymentVoucherTaxes.Any())
            {
                foreach (var taxCategory in request.PaymentVoucherTaxes)
                {
                    taxCategory.CreatedBy = request.CreatedBy;
                    taxCategory.CreatedDate = request.CreatedDate;
                    voucherEntity.AddPaymentVoucherTax(taxCategory);
                }
            }

            // Thêm chi tiết thanh toán
            if (request.PaymentDetails != null && request.PaymentDetails.Any())
            {

                foreach (var paymentDetail in request.PaymentDetails)
                {
                    bool force = request.PaymentDetails.Any(p => p.PaymentTurn == paymentDetail.PaymentTurn && p.PaidAmount > 0);
                    if (!force && paymentDetail.PaidAmount <= 0) continue;

                    paymentDetail.CreatedBy = request.CreatedBy;
                    voucherEntity.AddPaymentDetail(paymentDetail);
                }
            }

            if (request.CurrencyUnitCode == "VND")
            {
                voucherEntity.ExchangeRate = 1;
            }
            else
            {
                var erResult = await _exchangeRateGrpcService.ExchangeRate(request.CurrencyUnitCode, "VND");
                if (erResult.IsSuccess) voucherEntity.ExchangeRate = erResult.Value;
            }
            voucherEntity.ExchangeRateApplyDate = DateTime.Now;

            voucherEntity.CalculateTotal();

            voucherEntity.SetStatusId(request.StatusId);

            var savedVoucherEntityRsp = await _paymentVoucherRepository.CreateAndSave(voucherEntity);
            commandResponse.CombineResponse(savedVoucherEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }
            else
            {
                //danh sách phiếu thu liên quan
                if (request.ReceiptVouchers.Any())
                {
                    var listReceiptVouchers = new List<object>();
                    foreach (var item in request.ReceiptVouchers)
                    {
                        listReceiptVouchers.Add(new ReceiptVoucherInPaymentVoucher()
                        {
                            CreatedDate = item.CreatedDate,
                            CreatedBy = item.CreatedBy,
                            ReceiptVoucherId = item.ReceiptVoucherId,
                            PaymentVoucherId = savedVoucherEntityRsp.Result.Id

                        });
                    }
                    try
                    {
                        await _reciptVoucherInPaymentVoucherRepository.InsertBulk(listReceiptVouchers);
                    }
                    catch (Exception e)
                    {
                        throw new DebtDomainException(e.InnerException.Message);
                    }
                }
            }

            var paymentVoucher = savedVoucherEntityRsp.Result;
            for (int i = 0; i < request.PaymentVoucherDetails.Count; i++)
            {
                if (request.PaymentVoucherDetails[i].AttachmentFiles?.Any() ?? false)
                {
                    var attachmentFiles =
                        await _attachmentFileService.PersistentFiles(request.PaymentVoucherDetails[i].AttachmentFiles.Select(c => c.TemporaryUrl)
                        .ToArray());
                    if (attachmentFiles == null || !attachmentFiles.Any())
                        throw new DebtDomainException("An error has occured when save the attachments");

                    int index = 0;
                    foreach (var fileCommand in attachmentFiles)
                    {
                        fileCommand.Name = request.PaymentVoucherDetails[i].AttachmentFiles.ElementAt(index).Name;
                        fileCommand.CreatedBy = request.PaymentVoucherDetails[i].CreatedBy;
                        fileCommand.PaymentVoucherDetailId
                            = paymentVoucher.PaymentVoucherDetails
                                .First(r => r.IdentityGuid == request.PaymentVoucherDetails[i].IdentityGuid).Id;

                        fileCommand.PaymentVoucherId = paymentVoucher.Id;
                        var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                        index++;
                        if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                    }
                }
            }

            return commandResponse;
        }
    }
}
