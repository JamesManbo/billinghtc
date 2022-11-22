using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ApplicationUserEvents
{
    public class UpdateContractorIntegrationEvent : IntegrationEvent
    {
        public CustomerIntegrationEvent Customer { get; set; }
        public PartnerIntegrationEvent Partner { get; set; }

        public UpdateContractorIntegrationEvent()
        {
        }
    }

    public class CustomerIntegrationEvent
    {
        public string IdentityGuid { get; set; }
        public string UserName { get; set; }
        public string CustomerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; }
        public string TaxIdNo { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }

        public string AccountingCustomerCode { get; set; }
        public string ShortName { get; set; }
        public bool HasUpdate { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupIds { get; set; }
        public string ContractorGroupNames { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string ContractorIndustryIds { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string ContractorIndustryNames { get; set; }
    }

    public class PartnerIntegrationEvent
    {
        public string IdentityGuid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; }
        public string TaxIdNo { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public bool HasUpdate { get; set; }
        public int? ContractorStructureId { get; set; }
        public int? ContractorCategoryId { get; set; }
        public string ContractorGroupIds { get; set; }
        public string ContractorGroupNames { get; set; }
        public int? ContractorClassId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string ContractorIndustryIds { get; set; }
        public string ContractorStructureName { get; set; }
        public string ContractorCategoryName { get; set; }
        public string ContractorClassName { get; set; }
        public string ContractorTypeName { get; set; }
        public string ContractorIndustryNames { get; set; }
    }
}
