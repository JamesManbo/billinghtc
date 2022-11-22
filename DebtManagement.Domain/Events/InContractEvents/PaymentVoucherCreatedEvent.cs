using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.InContractEvents
{
    public class PaymentVoucherCreatedEvent : INotification
    {
        public int TargetId { get; set; }
        public decimal GrandTotal { get; set; }
        public int InContractId { get; set; }
        public List<PaymentVoucherDetail> VoucherDetails { get; set; }
        public DateTime NextBillingDate { get; set; }
    }
}
