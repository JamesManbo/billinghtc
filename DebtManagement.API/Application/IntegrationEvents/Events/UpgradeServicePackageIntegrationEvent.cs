using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.ContractModels;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class UpgradeServicePackageIntegrationEvent : IntegrationEvent
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public List<OutContractServicePackageDTO> NewOutContractServicePackages { get; set; }
    }
}
