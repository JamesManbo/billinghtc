using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Exceptions;
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
    public class CancelPaymentVoucherCommandHander : IRequestHandler<CancelPaymentVoucherCommand,
            ActionResponse>
    {
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;

        public CancelPaymentVoucherCommandHander(
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
        /// 
        public async Task<ActionResponse> Handle(CancelPaymentVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse();

            var paymentVoucher = await _paymentVoucherRepository.GetByIdAsync(request.Id);
            if (paymentVoucher == null)
            {
                throw new DebtDomainException("Phiếu đề nghị thanh toán không tồn tại");
            }

            if (paymentVoucher.StatusId != PaymentVoucherStatus.New.Id)
            {
                throw new DebtDomainException("Không thể hủy phiếu đề nghị thanh toán");
            }

            paymentVoucher.CancellationReason = request.CancellationReason;
            paymentVoucher.SetStatusId(PaymentVoucherStatus.Canceled.Id, request.UpdatedByUserId);
            paymentVoucher.UpdatedBy = request.UpdatedBy;
            paymentVoucher.UpdatedDate = DateTime.Now;

            var savedVoucherEntityRsp = await _paymentVoucherRepository.UpdateAndSave(paymentVoucher);
            commandResponse.CombineResponse(savedVoucherEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }

            return commandResponse;
        }
    }
}
