using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Validations.ReceiptVoucherValidator;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.PaymentVoucherCommandHandler
{
    public class CancelListPaymentVoucherCommandHander : IRequestHandler<CancelListPaymentVoucherCommand,
            ActionResponse>
    {
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;

        public CancelListPaymentVoucherCommandHander(
            IPaymentVoucherRepository paymentVoucherRepository)
        {
            _paymentVoucherRepository = paymentVoucherRepository;
        }

        /// <summary>
        /// Xử lý cập nhật phiếu thu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse> Handle(CancelListPaymentVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse();
           

            if (request.CancellationReason == null || request.CancellationReason == "")
            {
                commandResponse.AddError("Bạn cần nhập lý do hủy phiếu đề nghị thanh toán");
                return commandResponse;
            }

            var paymentVouchers = await _paymentVoucherRepository
                .GetByIdsAsync(request.Ids.Split(',')
                .Select(int.Parse)
                .ToArray());

            if (paymentVouchers.Any() == false)
            {
                throw new DebtDomainException("Phiếu đề nghị thanh toán không tồn tại");
            }

            foreach (var paymentVoucher in paymentVouchers.Where(o => o.StatusId == PaymentVoucherStatus.New.Id).ToList())
            {
                paymentVoucher.CancellationReason = request.CancellationReason;
                paymentVoucher.SetStatusId(PaymentVoucherStatus.Canceled.Id, request.UpdatedByUserId);
                paymentVoucher.UpdatedBy = request.UpdatedBy;
                paymentVoucher.UpdatedDate = DateTime.Now;
                var savedVoucherEntityRsp = _paymentVoucherRepository.Update(paymentVoucher);
                commandResponse.CombineResponse(savedVoucherEntityRsp);
            }

            await _paymentVoucherRepository.SaveChangeAsync();

            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }

            return commandResponse;
        }
    }
}
