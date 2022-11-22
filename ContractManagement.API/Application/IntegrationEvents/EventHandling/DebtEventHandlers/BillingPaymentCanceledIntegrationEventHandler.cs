using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Domain.Commands.DebtCommand;
using EventBus.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers
{
    public class BillingPaymentCanceledIntegrationEventHandler : IIntegrationEventHandler<BillingPaymentCanceledIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public BillingPaymentCanceledIntegrationEventHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(BillingPaymentCanceledIntegrationEvent @event)
        {
            var updateContractAfterBillingCanceledCmd = new UpdateContractAfterBillingCanceledCommand()
            {
                IsActiveSPST = @event.IsActiveSPST,
                OutContractId = @event.OutContractId,
                VoucherDetails = @event.VoucherDetails,
                Promotions = @event.Promotions
            };

            await _mediator.Send(updateContractAfterBillingCanceledCmd);
        }
    }
}
