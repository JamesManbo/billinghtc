using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using DebtManagement.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers
{
    public class PaidOpeningDebtDomainEventHandler : INotificationHandler<PaidOpeningDebtDomainEvent>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public PaidOpeningDebtDomainEventHandler(IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task Handle(PaidOpeningDebtDomainEvent noti, CancellationToken cancellationToken)
        {
            var inDebtVoucherIds = noti.OpeningDebtHistories
                .Where(p => p.ReceiptVoucherId.HasValue)
                .Select(p => p.ReceiptVoucherId.Value)
                .Distinct()
                .ToArray();

            var debtReceiptVouchers = await _receiptVoucherRepository.GetByIdsAsync(inDebtVoucherIds);

            if (debtReceiptVouchers != null && debtReceiptVouchers.Any())
            {
                foreach (var unpaidVoucher in debtReceiptVouchers)
                {
                    unpaidVoucher.PassiveDebtPayingHandler(
                        noti.ReceiptVoucherId,
                        noti.ApprovedUserId,
                        noti.AccountingCode,
                        noti.InvoiceCode,
                        noti.InvoiceDate,
                        noti.InvoiceReceivedDate,
                        noti.PaymentDate
                        );
                }

                await _receiptVoucherRepository.SaveChangeAsync();
            }
        }
    }
}
