using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.ContractEvents
{
    public class TerminateServicePackagesDomainEvent : INotification
    {
        public List<int> OutContractServicePackageIds { get; set; }
        public TerminateServicePackagesDomainEvent()
        {
            OutContractServicePackageIds = new List<int>();
        }
    }
}
