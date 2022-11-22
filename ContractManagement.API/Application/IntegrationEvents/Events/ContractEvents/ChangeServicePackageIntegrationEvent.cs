using ContractManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class ChangeServicePackageIntegrationEvent : IntegrationEvent
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public OutContractServicePackageDTO OldOutContractServicePackage { get; set; }
        public OutContractServicePackageDTO NewOutContractServicePackage { get; set; }
    }
}
