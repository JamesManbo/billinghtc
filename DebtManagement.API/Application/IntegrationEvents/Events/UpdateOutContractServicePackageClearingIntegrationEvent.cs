using DebtManagement.Domain.Events.ContractEvents;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events
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
