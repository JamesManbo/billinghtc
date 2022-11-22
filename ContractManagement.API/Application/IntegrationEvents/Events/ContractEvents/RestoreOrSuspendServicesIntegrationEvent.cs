using ContractManagement.Domain.Events.DebtEvents;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents
{
    public class RestoreOrSuspendServicesIntegrationEvent : IntegrationEvent
    {
        public bool IsActive { get; set; }
        public List<OutContractServicePackageIntegrationEvent> OutContractServicePackageEvents { get; set; }

        public RestoreOrSuspendServicesIntegrationEvent(bool isActive, List<OutContractServicePackageIntegrationEvent> outContractServicePackageEvents)
        {
            IsActive = isActive;
            OutContractServicePackageEvents = outContractServicePackageEvents;
        }
    }
}
