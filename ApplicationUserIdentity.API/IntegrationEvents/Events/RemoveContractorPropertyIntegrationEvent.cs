using ApplicationUserIdentity.API.Models.Commands.ContractorCommands;
using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUserIdentity.API.IntegrationEvents.Events
{
    public class RemoveContractorPropertyIntegrationEvent : IntegrationEvent
    {
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public int? ContractorGroupId { get; set; }
        public string ContractorGroupName { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public int? ContractorIndustryId { get; set; }

        public RemoveContractorPropertyIntegrationEvent(RemoveContractorPropertyDomainEvent domainEvent)
        {
            this.ContractorStructureId = domainEvent.ContractorStructureId;
            this.ContractorCategoryId = domainEvent.ContractorCategoryId;
            this.ContractorGroupId = domainEvent.ContractorGroupId;
            this.ContractorGroupName = domainEvent.ContractorGroupName;
            this.ContractorClassId = domainEvent.ContractorClassId;
            this.ContractorTypeId = domainEvent.ContractorTypeId;
            this.ContractorIndustryId = domainEvent.ContractorIndustryId;
        }
        public RemoveContractorPropertyIntegrationEvent()
        {
        }
    }
}
