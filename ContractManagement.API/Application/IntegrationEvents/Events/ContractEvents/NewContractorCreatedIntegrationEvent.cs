using EventBus.Events;

namespace ContractManagement.API.Application.IntegrationEvents.Events
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

        public NewContractorCreatedIntegrationEvent(string identityGuid, string fullName)
        {
            IdentityGuid = identityGuid;
            FullName = fullName;
        }
    }
}
