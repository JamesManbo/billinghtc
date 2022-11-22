using ContractManagement.Domain.Events.ContractEvents;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events
{
    public class UpdateServicePackageSuspensionTimesIntegrationEvent: IntegrationEvent
    {
        public UpdateServicePackageSuspensionTimesIntegrationEvent(List<ServicePackageSuspensionTimeEvent> servicePackageSuspensionTimeEvents)
        {
            ServicePackageSuspensionTimeEvents = servicePackageSuspensionTimeEvents;
        }

        public List<ServicePackageSuspensionTimeEvent> ServicePackageSuspensionTimeEvents { get; set; }
    }
}
