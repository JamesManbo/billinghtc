using DebtManagement.API.Application.IntegrationEvents.Events.VoucherTargetEvents;
using DebtManagement.Domain.Commands.VoucherTargetCommand;
using EventBus.Abstractions;
using MediatR;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling.VoucherTargets
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
                StructureId = @event.ContractorStructureId,
                CategoryId = @event.ContractorCategoryId,
                GroupId = @event.ContractorGroupId,
                GroupName = @event.ContractorGroupName,
                ClassId = @event.ContractorClassId,
                TypeId = @event.ContractorTypeId,
                IndustryId = @event.ContractorIndustryId,
            };

            await this._mediator.Send(command);
        }
    }
}
