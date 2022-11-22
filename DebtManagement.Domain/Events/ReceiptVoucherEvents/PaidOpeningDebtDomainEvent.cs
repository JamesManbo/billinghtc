using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ReceiptVoucherEvents
{
    public class PaidOpeningDebtDomainEvent : INotification
    {
        public string ReceiptVoucherId { get; set; }
        public IEnumerable<ReceiptVoucherDebtHistory> OpeningDebtHistories { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceReceivedDate{ get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public string ApprovedUserId { get; set; }
        public string AccountingCode { get; set; }
        public string InvoiceCode { get; set; }
    }
}
