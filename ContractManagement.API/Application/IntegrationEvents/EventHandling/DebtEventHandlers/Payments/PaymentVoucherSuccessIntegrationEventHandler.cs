using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers.Payments
{
    public class PaymentVoucherSuccessIntegrationEventHandler : IIntegrationEventHandler<PaymentVoucherSuccessIntegrationEvent>
    {
        public Task Handle(PaymentVoucherSuccessIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
