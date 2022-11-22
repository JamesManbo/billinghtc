using ContractManagement.API.Application.IntegrationEvents.Events.ContractorEvents;
using ContractManagement.Domain.Commands.ContractorCommand;
using EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.EventHandling.ContractorEventHandlers
{
    public class UpdateContractorPropertyIntegrationEventHandler : IIntegrationEventHandler<UpdateContractorPropertyIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public UpdateContractorPropertyIntegrationEventHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(UpdateContractorPropertyIntegrationEvent @event)
        {
            var command = new UpdateContractorPropertyCommand
            {
                ContractorId = @event.ContractorId,
                ContractorStructureId = @event.ContractorStructureId,
                ContractorCategoryId = @event.ContractorCategoryId,
                ContractorGroupId = @event.ContractorGroupId,
                OldContractorGroupName = @event.OldContractorGroupName,
                NewContractorGroupName = @event.NewContractorGroupName,
                ContractorClassId = @event.ContractorClassId,
                ContractorTypeId = @event.ContractorTypeId,
                ContractorIndustryId = @event.ContractorIndustryId,
                ContractorStructureName = @event.ContractorStructureName,
                ContractorCategoryName = @event.ContractorCategoryName,
                ContractorClassName = @event.ContractorClassName,
                ContractorTypeName = @event.ContractorTypeName,
                OldContractorIndustryName = @event.OldContractorIndustryName,
                NewContractorIndustryName = @event.NewContractorIndustryName,
                ApplicationUserIdentityGuid = @event.ApplicationUserIdentityGuid
            };

            await this._mediator.Send(command);
        }
    }
}
