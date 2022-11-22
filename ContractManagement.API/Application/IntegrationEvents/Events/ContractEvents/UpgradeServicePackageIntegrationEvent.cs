using ContractManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class UpgradeServicePackageIntegrationEvent : IntegrationEvent
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public List<OutContractServicePackageDTO> NewOutContractServicePackages { get; set; }
    }
}
