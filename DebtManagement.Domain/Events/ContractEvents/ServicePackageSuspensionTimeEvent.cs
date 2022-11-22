using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ContractEvents
{
    public class ServicePackageSuspensionTimeEvent
    {
        public int Id { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
