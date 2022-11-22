using ApplicationUserIdentity.API.Application.Commands.SynchronizeData;
using ApplicationUserIdentity.API.IntegrationEvents.EventModels;
using EventBus.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.IntegrationEvents.EventHandling
{
    public class InsertBulkContractorEventHandler : IIntegrationEventHandler<InsertBulkApplicationUserFromContractorIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public InsertBulkContractorEventHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(InsertBulkApplicationUserFromContractorIntegrationEvent @event)
        {
            var command = new SyncContractorFromBulkInsertContractCommand(@event.FromId);
            await _mediator.Send(command);
        }
    }
}
