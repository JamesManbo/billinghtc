using EventBus.Events;
using System;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Receipts
{
    public class VoucherBillingDateChangeIntegrationEvent : IntegrationEvent
    {
        public VoucherBillingDateChangeIntegrationEvent()
        {
        }

        public int ChannelId { get; set; }
        public DateTime OldEndingBillingDate { get; set; }
        public DateTime NewEndingBillingDate { get; set; }
    }
}
