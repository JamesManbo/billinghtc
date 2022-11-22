using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Seed;
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
    public class CancelReceiptVoucherCommandHandler : IRequestHandler<CancelReceiptVoucherCommand,
            ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public CancelReceiptVoucherCommandHandler(
            IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        /// <summary>
        /// Xử lý cập nhật phiếu thu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse> Handle(CancelReceiptVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse();

            var receiptVoucher = await _receiptVoucherRepository.GetByIdAsync(request.Id);
            if (receiptVoucher == null)
            {
                throw new DebtDomainException("Phiếu thu không tồn tại");
            }

            if (receiptVoucher.StatusId == ReceiptVoucherStatus.Canceled.Id ||
                receiptVoucher.StatusId == ReceiptVoucherStatus.Invoiced.Id ||
                receiptVoucher.StatusId == ReceiptVoucherStatus.PayingBadDebt.Id ||
                receiptVoucher.StatusId == ReceiptVoucherStatus.Success.Id)
            {
                commandResponse.AddError($"Không thể hủy phiếu thu có trạng thái là {Enumeration.FromValue<ReceiptVoucherStatus>(receiptVoucher.StatusId)}");
                return commandResponse;
            }

            receiptVoucher.CancellationReason = request.CancellationReason;
            receiptVoucher.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
            receiptVoucher.UpdatedBy = request.UpdatedBy;
            receiptVoucher.UpdatedDate = DateTime.Now;

            var savedVoucherEntityRsp = await _receiptVoucherRepository.UpdateAndSave(receiptVoucher);
            commandResponse.CombineResponse(savedVoucherEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }

            return commandResponse;
        }
    }
}
