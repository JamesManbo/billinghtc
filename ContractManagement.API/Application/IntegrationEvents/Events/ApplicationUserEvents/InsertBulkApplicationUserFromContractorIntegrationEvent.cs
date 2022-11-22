using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ApplicationUserEvents
{
    public class InsertBulkApplicationUserFromContractorIntegrationEvent : IntegrationEvent
    {
        public int FromId { get; set; }

        public InsertBulkApplicationUserFromContractorIntegrationEvent(int fromId)
        {
            FromId = fromId;
        }
    }
}
