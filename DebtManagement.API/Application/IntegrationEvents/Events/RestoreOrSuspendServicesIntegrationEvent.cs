using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events
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
