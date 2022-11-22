using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ReceiptVoucherEvents
{
    public class CollectingOpeningDebtDomainEvent : INotification
    {
        public string ReceiptVoucherId { get; set; }
        public IEnumerable<ReceiptVoucherDebtHistory> OpeningDebtPayments { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate { get; set; }
    }
}
