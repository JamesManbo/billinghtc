using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.API.Application.IntegrationEvents.Events.VoucherTargetEvents
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
