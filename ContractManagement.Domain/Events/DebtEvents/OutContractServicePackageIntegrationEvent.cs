using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.DebtEvents
{
    public class OutContractServicePackageIntegrationEvent
    {
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public int PaymentPeriod { get; set; }
        public DateTime? NextBilling { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public int? ServicePackageSuspensionTimeId { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
    }
}
