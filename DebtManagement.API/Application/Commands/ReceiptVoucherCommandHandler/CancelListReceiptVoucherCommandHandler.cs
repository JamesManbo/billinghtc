using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
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
    public class CancelListReceiptVoucherCommandHandler : IRequestHandler<CancelListReceiptVoucherCommand,
            ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public CancelListReceiptVoucherCommandHandler(
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
        public async Task<ActionResponse> Handle(CancelListReceiptVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse();
            try
            {
                var receiptVouchers = await _receiptVoucherRepository
               .GetByIdsAsync(request.Ids.Split(',').Select(int.Parse).ToArray());
                if (receiptVouchers.Any() == false)
                {
                    throw new DebtDomainException("Phiếu thu không tồn tại");
                }
                // thêm trường hợp được hủy khi trạng thái đang là thu hộ
                foreach (var receiptVoucher in receiptVouchers.Where(o => o.StatusId == ReceiptVoucherStatus.Pending.Id || o.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id).ToList())
                {
                    receiptVoucher.CancellationReason = request.CancellationReason;
                    receiptVoucher.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                    receiptVoucher.UpdatedBy = request.UpdatedBy;
                    receiptVoucher.UpdatedDate = DateTime.Now;
                    var savedVoucherEntityRsp = _receiptVoucherRepository.Update(receiptVoucher);
                    commandResponse.CombineResponse(savedVoucherEntityRsp);
                }

                await _receiptVoucherRepository.SaveChangeAsync();

                if (!commandResponse.IsSuccess)
                {
                    throw new DebtDomainException(commandResponse.Message);
                }

                return commandResponse;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
