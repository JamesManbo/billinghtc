using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.DebtCommandHandler
{
    public class CollectingOpeningDebtCommandHandler : IRequestHandler<CollectingOpeningDebtCommand, ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public CollectingOpeningDebtCommandHandler(
            IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task<ActionResponse> Handle(CollectingOpeningDebtCommand request, CancellationToken cancellationToken)
        {
            if (request.Event.OpeningDebtPayments != null
                && request.Event.OpeningDebtPayments.Any())
            {
                var inDebtVoucherIds = request.Event.OpeningDebtPayments
                    .Where(p => p.Status != PaymentStatus.Accounted && p.ReceiptVoucherId.HasValue)
                    .Select(p => p.ReceiptVoucherId.Value)
                    .ToArray();

                if (inDebtVoucherIds != null && inDebtVoucherIds.Any())
                {
                    var debtReceiptVouchers = await _receiptVoucherRepository.GetByIdsAsync(inDebtVoucherIds);
                    if (debtReceiptVouchers != null && debtReceiptVouchers.Any())
                    {
                        foreach (var unpaidVoucher in debtReceiptVouchers)
                        {
                            unpaidVoucher.PassiveDebtCollectingHandler(
                                request.Event.IssuedDate);
                        }

                        await _receiptVoucherRepository.SaveChangeAsync();
                    }
                }
            }

            return ActionResponse.Success;
        }
    }
}
