using ContractManagement.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.ContractEvents
{
    public class UpgradeServicePackageDomainEvent : INotification
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public List<OutContractServicePackageDTO> NewOutContractServicePackages { get; set; }

        public UpgradeServicePackageDomainEvent()
        {
            NewOutContractServicePackages = new List<OutContractServicePackageDTO>();
        }
    }
}
