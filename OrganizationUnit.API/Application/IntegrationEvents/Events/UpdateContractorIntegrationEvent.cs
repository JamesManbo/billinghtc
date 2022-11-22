using OrganizationUnit.Domain.Models.ApplicationUser;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.API.Application.IntegrationEvents.Events.ApplicationUserEvents
{
    public class UpdateContractorIntegrationEvent : IntegrationEvent
    {
        public CustomerIntegrationEvent Customer { get; set; }
        public PartnerIntegrationEvent Partner { get; set; }

        public UpdateContractorIntegrationEvent(CustomerIntegrationEvent customer, PartnerIntegrationEvent partner)
        {
            Customer = customer;
            Partner = partner;
        }
    }

    public class CustomerIntegrationEvent
    {
        public string IdentityGuid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; }
        public string TaxIdNo { get; set; }
        public string Address { get; set; }
        public string AccountingCustomerCode { get; set; }
        public bool HasUpdate { get; set; }
    }

    public class PartnerIntegrationEvent
    {
        public string IdentityGuid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; }
        public string TaxIdNo { get; set; }
        public string Address { get; set; }
        public bool HasUpdate { get; set; }
    }
}
