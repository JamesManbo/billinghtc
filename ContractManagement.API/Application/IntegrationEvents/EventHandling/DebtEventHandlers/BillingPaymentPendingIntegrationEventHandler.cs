using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers
{
    public class BillingPaymentPendingIntegrationEventHandler : IIntegrationEventHandler<BillingPaymentPendingIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public BillingPaymentPendingIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(BillingPaymentPendingIntegrationEvent @event)
        {
            var updateNextBillingDateCmd = new UpdateContractAfterBillingCommand()
            {
                IsActiveSPST = @event.IsActiveSPST,
                VoucherDetails = @event.VoucherDetails,
                Promotions = @event.Promotions
            };

            await _mediator.Send(updateNextBillingDateCmd);
        }
    }
}
