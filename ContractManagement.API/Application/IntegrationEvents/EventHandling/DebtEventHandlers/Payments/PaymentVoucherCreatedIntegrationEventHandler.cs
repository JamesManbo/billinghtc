using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers.Payments
{
    public class PaymentVoucherCreatedIntegrationEventHandler : IIntegrationEventHandler<PaymentVoucherCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public PaymentVoucherCreatedIntegrationEventHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(PaymentVoucherCreatedIntegrationEvent @event)
        {
            await this._mediator.Send(@event);
        }
    }
}
