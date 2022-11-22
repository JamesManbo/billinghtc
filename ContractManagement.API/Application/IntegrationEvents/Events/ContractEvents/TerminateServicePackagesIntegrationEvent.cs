using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class TerminateServicePackagesIntegrationEvent : IntegrationEvent
    {
        public List<int> OutContractServicePackageIds { get; set; }
        public TerminateServicePackagesIntegrationEvent()
        {
            OutContractServicePackageIds = new List<int>();
        }
    }
}
