using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ContractEvents
{
    public class OutContractServicePackageClearingIntegrationEvent
    {
        public decimal Change { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsDaysPlus { get; set; }
    }
}
