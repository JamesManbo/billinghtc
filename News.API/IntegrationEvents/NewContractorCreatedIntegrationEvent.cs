using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Events;

namespace News.API.IntegrationEvents
{
    public class NewContractorCreatedIntegrationEvent : IntegrationEvent
    {
        public string IdentityGuid { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string WardIdentityGuid { get; set; }
        public string District { get; set; }
        public string DistrictIdentityGuid { get; set; }
        public string Province { get; set; }
        public string ProvinceIdentityGuid { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }

        public NewContractorCreatedIntegrationEvent(string identityGuid)
        {
            IdentityGuid = identityGuid;
        }
    }
}
