using ContractManagement.API.Application.IntegrationEvents.Events.ContractorEvents;
using ContractManagement.Domain.Commands.ContractorCommand;
using EventBus.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.ContractorEventHandlers
{
    public class RemoveContractorPropertyIntegrationEventHandler : IIntegrationEventHandler<RemoveContractorPropertyIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public RemoveContractorPropertyIntegrationEventHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(RemoveContractorPropertyIntegrationEvent @event)
        {
            var command = new RemoveContractorPropertyCommand
            {
                ContractorStructureId = @event.ContractorStructureId,
                ContractorCategoryId = @event.ContractorCategoryId,
                ContractorGroupId = @event.ContractorGroupId,
                ContractorGroupName = @event.ContractorGroupName,
                ContractorClassId = @event.ContractorClassId,
                ContractorTypeId = @event.ContractorTypeId,
                ContractorIndustryId = @event.ContractorIndustryId,
            };

            await this._mediator.Send(command);
        }
    }
}
