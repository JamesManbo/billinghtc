using ContractManagement.Domain.Events.DebtEvents;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents
{
    public class UpdateOutContractServicePackageClearingIntegrationEvent : IntegrationEvent
    {
        public UpdateOutContractServicePackageClearingIntegrationEvent(List<OutContractServicePackageClearingIntegrationEvent> outContractServicePackageClearingIntegrationEvents)
        {
            OutContractServicePackageClearingIntegrationEvents = outContractServicePackageClearingIntegrationEvents;
        }

        public List<OutContractServicePackageClearingIntegrationEvent> OutContractServicePackageClearingIntegrationEvents { get; set; }
    }
}
