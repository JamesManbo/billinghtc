using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ReceiptVoucherEvents
{
    public class VoucherBillingDateChangeDomainEvent : INotification
    {
        public int ChannelId { get; set; }
        public DateTime OldEndingBillingDate { get; set; }
        public DateTime NewEndingBillingDate { get; set; }

        public VoucherBillingDateChangeDomainEvent()
        {
        }
    }
}
