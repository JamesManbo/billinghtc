using ContractManagement.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.ContractEvents
{
    public class ChangeServicePackageDomainEvent : INotification
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public OutContractServicePackageDTO OldOutContractServicePackage { get; set; }
        public OutContractServicePackageDTO NewOutContractServicePackage { get; set; }
    }
}
