using AutoMapper;
using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
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

namespace DebtManagement.API.Application.Commands.ReceiptVoucherCommandHandler
{
    public class
        UpdateReceiptVoucherCommandHandler : IRequestHandler<UpdateReceiptVoucherCommand,
            ActionResponse<ReceiptVoucherDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IOutContractService _outContractService;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IConfigurationSystemParameterGrpcService _configurationSystemParameterService;
        private readonly IFeedbackGrpcService _feedbackGrpcService;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public UpdateReceiptVoucherCommandHandler(IMediator mediator,
            IReceiptVoucherRepository receiptVoucherRepository,
            IFeedbackGrpcService feedbackGrpcService,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IAttachmentFileRepository fileRepository,
            IConfigurationSystemParameterGrpcService configurationSystemParameterService,
            IMapper mapper, IOutContractService outContractService)
        {
            _mediator = mediator;
            _receiptVoucherRepository = receiptVoucherRepository;
            _feedbackGrpcService = feedbackGrpcService;
            _configurationSystemParameterService = configurationSystemParameterService;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _outContractService = outContractService;
        }

        /// <summary>
        /// Xử lý cập nhật phiếu thu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<ReceiptVoucherDTO>> Handle(UpdateReceiptVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var systemConfiguration = await _configurationSystemParameterService.GetSystemConfigurations();
            var commandResponse = new ActionResponse<ReceiptVoucherDTO>();
            if (_receiptVoucherRepository.IsExistByCode(request.VoucherCode, request.Id))
            {
                commandResponse.AddError("Số phiếu thu đã tồn tại ", nameof(request.VoucherCode));
                return commandResponse;
            }
            if (!string.IsNullOrEmpty(request.InvoiceCode) && _receiptVoucherRepository.IsExistByInvoice(request.InvoiceCode, request.Id))
            {
                commandResponse.AddError("Số hóa đơn đã tồn tại ", nameof(request.InvoiceCode));
                return commandResponse;
            }

            var voucherEntity = await _receiptVoucherRepository.GetByIdAsync(request.Id);
            if (voucherEntity.IsLock == true)
            {
                commandResponse.AddError("Phiếu thu đang bị khóa");
                return commandResponse;
            }

            //promotion
            if (request.PromotionForReceiptNews.Count > 0)
            {
                foreach (var item in request.PromotionForReceiptNews)
                {
                    var promo = _mapper.Map<PromotionForReceiptVoucher>(item);
                    promo.CreatedDate = DateTime.Now;
                    voucherEntity.SetPromotionForReceiptVoucher(promo);
                }
            }

            if (request.PromotionForReceiptDels.Count > 0)
            {
                var Ids = new List<int>();
                foreach (var item in request.PromotionForReceiptDels)
                {
                    Ids.Add(item.Id);
                }
                voucherEntity.RemovePromotion(Ids);
            }

            // Chỉ cập nhật các trường thông tin giới hạn cho phép
            voucherEntity.Update(request);

            // Cập nhật danh sách chi tiết phiếu thu 
            if (request.ReceiptLines != null && request.ReceiptLines.Count > 0)
            {
                //request.ReceiptLines.ForEach(voucherEntity.UpdateReceiptVoucherDetail);
                for (int i = 0; i < request.ReceiptLines.Count; i++)
                {
                    voucherEntity.UpdateReceiptVoucherDetail(request.ReceiptLines[i]);

                    if (request.ReceiptLines[i].AttachmentFiles != null
                    && request.ReceiptLines[i].AttachmentFiles.Any(f => !string.IsNullOrWhiteSpace(f.TemporaryUrl)))
                    {
                        var attachmentFiles =
                            await _attachmentFileService.PersistentFiles(request.ReceiptLines[i].AttachmentFiles.Select(c => c.TemporaryUrl)
                                .ToArray());
                        if (attachmentFiles == null || !attachmentFiles.Any())
                            throw new DebtDomainException("An error has occured when save the attachment files");

                        for (int j = 0; j < request.ReceiptLines[i].AttachmentFiles.Count; j++)
                        {
                            var fileCommand = request.ReceiptLines[i].AttachmentFiles.ElementAt(j);
                            var uploadedRspFile = attachmentFiles
                                .Find(e => e.TemporaryUrl.Equals(fileCommand.TemporaryUrl,
                                    StringComparison.InvariantCultureIgnoreCase));

                            fileCommand.FileName = uploadedRspFile.FileName;
                            fileCommand.FilePath = uploadedRspFile.FilePath;
                            fileCommand.Name = uploadedRspFile.Name;
                            fileCommand.FileType = uploadedRspFile.FileType;
                            fileCommand.Size = uploadedRspFile.Size;
                            fileCommand.Extension = uploadedRspFile.Extension;

                            fileCommand.CreatedBy = voucherEntity.CreatedBy;
                            fileCommand.ReceiptVoucherId = voucherEntity.Id;
                            fileCommand.ReceiptVoucherDetailId = request.ReceiptLines[i].Id;

                            ActionResponse<AttachmentFile> savedFileRsp;
                            if (fileCommand.Id == 0)
                            {
                                savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                            }
                            else
                            {
                                savedFileRsp = await _fileRepository.UpdateAndSave(fileCommand);
                            }

                            if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                        }
                    }

                    if (request.ReceiptLines[i].DeletedAttachments != null)
                    {
                        foreach (var attachmentId in request.ReceiptLines[i].DeletedAttachments)
                        {
                            _fileRepository.DeleteAndSave(attachmentId);
                        }
                    }
                }

            }

            // Cập nhật chi tiết thanh toán công nợ phát sinh
            if (request.IncurredDebtPayments != null && request.IncurredDebtPayments.Any())
            {
                request.IncurredDebtPayments.ForEach(voucherEntity.UpdatePaymentDetail);
            }

            // Cập nhật chi tiết thanh toán công nợ đầu kỳ
            if (request.OpeningDebtPayments != null
                && request.OpeningDebtPayments.Any())
            {
                request.OpeningDebtPayments.ForEach(voucherEntity.UpdateDebtPaymentDetail);
            }
            voucherEntity.CalculateTotal(true);
            voucherEntity.CalculatePaidTotal();

            voucherEntity.SetStatusId(request.StatusId);

            // Kiểm tra và chuyển trạng thái của phiếu thu sang Nợ quá hạn hoặc Nợ Xấu           
            voucherEntity.UpdateStatusOverdue();
            voucherEntity.UpdateStatusBadDebt(systemConfiguration.NumberDaysBadDebt ?? 30);

            var savedVoucherEntityRsp = await _receiptVoucherRepository.UpdateAndSave(voucherEntity);
            commandResponse.CombineResponse(savedVoucherEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }

            //update mongo feedback
            if (request.ReceiptLines != null && request.ReceiptLines.Count > 0)
            {
                foreach (var voucherDetail in request.ReceiptLines)
                {
                    if (voucherDetail.ReductionDetails != null
                        && voucherDetail.ReductionDetails.Any())
                    {

                        await _feedbackGrpcService.UpdateReceiptLine(string.Join("##", voucherDetail.ReductionDetails.Select(r => r.ReasonId)), voucherDetail.Id);

                        var updateModel = voucherEntity.ReceiptVoucherDetails.FirstOrDefault(r => r.Id == voucherDetail.Id);
                        var removedIds = updateModel.ReceiptVoucherDetailReductions.Where(t =>
                            voucherDetail.ReductionDetails.All(c => c.ReasonId != t.ReasonId))
                            .Select(t => t.ReasonId);
                        await _feedbackGrpcService.UpdateReceiptLine(string.Join("##", removedIds), 0);
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

            // Update thứ hạng của khách hàng dựa trên doanh thu
            // Sonnd 2021-06-28 Comment: Cơ chế thay đổi thứ hạng của khách hàng sẽ dựa theo nhiều tiêu chí và được người dùng thao tác manual
            //_receiptVoucherRepository.UpdateApplicationUserClass(request.TargetId, request.OutContractId);

            return commandResponse;
        }

    }
}