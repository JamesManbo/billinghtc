using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ClearingCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.ClearingCommandHandler
{
    public class RejectClearingCommandHandler : IRequestHandler<RejectClearingVoucherCommand, ActionResponse>
    {
        private readonly IClearingRepository _clearingRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _attachmentFileRepository;

        public RejectClearingCommandHandler(IClearingRepository clearingRepository, IAttachmentFileResourceGrpcService attachmentFileService, IAttachmentFileRepository attachmentFileRepository)
        {
            this._clearingRepository = clearingRepository;
            this._attachmentFileService = attachmentFileService;
            this._attachmentFileRepository = attachmentFileRepository;
        }

        public async Task<ActionResponse> Handle(RejectClearingVoucherCommand request, 
            CancellationToken cancellationToken)
        {
            var clearingVoucher = await _clearingRepository.GetByIdAsync(request.Id);
            clearingVoucher.StatusId = ClearingStatus.Cancelled.Id;
            clearingVoucher.ApprovalUserId = request.ApprovedByUserId;
            clearingVoucher.CancelReason = request.Reason;

            if (clearingVoucher.ReceiptVouchers.Any())
            {
                foreach (var receiptVch in clearingVoucher.ReceiptVouchers)
                {
                    if (receiptVch.TypeId == ReceiptVoucherType.Clearing.Id)
                    {
                        receiptVch.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                        receiptVch.CancellationReason = $"Hủy cùng với biên bản bù trừ số {clearingVoucher.CodeClearing}.";
                    }
                    else
                    {
                        receiptVch.IsLock = false;
                        receiptVch.ClearingId = null;
                        receiptVch.UpdatedDate = DateTime.UtcNow;
                        receiptVch.UpdatedBy = request.ApprovedBy;
                    }
                }
            }

            if (clearingVoucher.PaymentVouchers.Any())
            {
                foreach (var paymentVch in clearingVoucher.PaymentVouchers)
                {
                    if (paymentVch.TypeId == ReceiptVoucherType.Clearing.Id)
                    {
                        paymentVch.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                        paymentVch.CancellationReason = $"Hủy cùng với biên bản bù trừ số {clearingVoucher.CodeClearing}.";
                    }
                    else
                    {
                        paymentVch.IsLock = false;
                        paymentVch.ClearingId = null;
                        paymentVch.UpdatedDate = DateTime.UtcNow;
                        paymentVch.UpdatedBy = request.ApprovedBy;
                    }
                }
            }

            if (request.AttachmentFiles != null && request.AttachmentFiles.Any())
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(request.AttachmentFiles.Select(c => c.TemporaryUrl)
                    .ToArray());
                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new DebtDomainException("An error has occured when save the attachments");

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.CreatedBy = request.ApprovedBy;
                    fileCommand.ClearingVoucherId = clearingVoucher.Id;
                    var savedFileRsp = await _attachmentFileRepository.CreateAndSave(fileCommand);
                    if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                }
            }

            await _clearingRepository.SaveChangeAsync();

            return ActionResponse.Success;
        }
    }
}
