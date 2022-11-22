using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.DebtEvents
{
    public class OutContractServicePackageClearingIntegrationEvent
    {
        public decimal Change { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsDaysPlus { get; set; }
    }
}
