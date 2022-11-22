using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.API.Application.IntegrationEvents.Events.VoucherTargetEvents
{
    public class UpdateContractorPropertyIntegrationEvent : IntegrationEvent
    {
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
