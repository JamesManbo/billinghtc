using DebtManagement.API.Application.IntegrationEvents.Events.VoucherTargetEvents;
using DebtManagement.Domain.Commands.VoucherTargetCommand;
using EventBus.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling.VoucherTargets
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
                StructureId = @event.ContractorStructureId,
                CategoryId = @event.ContractorCategoryId,
                GroupId = @event.ContractorGroupId,
                OldGroupName = @event.OldContractorGroupName,
                NewGroupName = @event.NewContractorGroupName,
                ClassId = @event.ContractorClassId,
                TypeId = @event.ContractorTypeId,
                IndustryId = @event.ContractorIndustryId,
                StructureName = @event.ContractorStructureName,
                CategoryName = @event.ContractorCategoryName,
                ClassName = @event.ContractorClassName,
                TypeName = @event.ContractorTypeName,
                OldIndustryName = @event.OldContractorIndustryName,
                NewIndustryName = @event.NewContractorIndustryName,
                ApplicationUserIdentityGuid = @event.ApplicationUserIdentityGuid
            };

            await this._mediator.Send(command);
        }
    }
}
