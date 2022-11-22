using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using EventBus.Events;
using System;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class VoucherBillingDateChangeIntegrationEvent : IntegrationEvent
    {
        public VoucherBillingDateChangeIntegrationEvent()
        {
        }

        public VoucherBillingDateChangeIntegrationEvent(VoucherBillingDateChangeDomainEvent domainEvent)
        {
            this.ChannelId = domainEvent.ChannelId;
            this.OldEndingBillingDate = domainEvent.OldEndingBillingDate;
            this.NewEndingBillingDate = domainEvent.NewEndingBillingDate;
        }

        public int ChannelId { get; set; }
        public DateTime OldEndingBillingDate { get; set; }
        public DateTime NewEndingBillingDate { get; set; }
    }
}
