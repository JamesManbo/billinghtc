using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ContractorEvents
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
    }
}
