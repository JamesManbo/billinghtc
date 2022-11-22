using ApplicationUserIdentity.API.Models.Commands.ContractorCommands;
using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUserIdentity.API.IntegrationEvents.Events
{
    public class UpdateContractorPropertyIntegrationEvent : IntegrationEvent
    {
        public UpdateContractorPropertyIntegrationEvent()
        {
        }

        public UpdateContractorPropertyIntegrationEvent(UpdateContractorPropertyDomainEvent domainEvent)
        {
            this.IsUpdateAll = domainEvent.IsUpdateAll;
            this.ContractorId = domainEvent.ContractorId;
            this.ContractorStructureId = domainEvent.ContractorStructureId;
            this.ContractorCategoryId = domainEvent.ContractorCategoryId;
            this.ContractorGroupId = domainEvent.ContractorGroupId;
            this.OldContractorGroupName = domainEvent.OldContractorGroupName;
            this.NewContractorGroupName = domainEvent.NewContractorGroupName;
            this.ContractorClassId = domainEvent.ContractorClassId;
            this.ContractorTypeId = domainEvent.ContractorTypeId;
            this.ContractorIndustryId = domainEvent.ContractorIndustryId;
            this.ContractorStructureName = domainEvent.ContractorStructureName;
            this.ContractorCategoryName = domainEvent.ContractorCategoryName;
            this.ContractorClassName = domainEvent.ContractorClassName;
            this.ContractorTypeName = domainEvent.ContractorTypeName;
            this.OldContractorIndustryName = domainEvent.OldContractorIndustryName;
            this.NewContractorIndustryName = domainEvent.NewContractorIndustryName;
            this.ApplicationUserIdentityGuid = domainEvent.ApplicationUserIdentityGuid;
        }

        /// <summary>
        /// Trường đánh dấu command có update tất cả các properties hay ko
        /// </summary>
        public bool IsUpdateAll { get; set; }
        public int ContractorId { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupId { get; set; }
        public string OldContractorGroupName { get; set; }
        public string NewContractorGroupName { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string ContractorIndustryId { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string OldContractorIndustryName { get; set; }
        public string NewContractorIndustryName { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }

    }
}
