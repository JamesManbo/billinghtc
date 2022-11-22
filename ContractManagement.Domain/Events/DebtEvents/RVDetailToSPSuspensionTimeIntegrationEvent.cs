using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.DebtEvents
{
        public class RVDetailToSPSuspensionTimeIntegrationEvent
    {
        public decimal DiscountAmountSuspend { get; set; }
        public string SPSuspensionTimeIds { get; set; }

        public RVDetailToSPSuspensionTimeIntegrationEvent(decimal discountAmountSuspend, string sPSuspensionTimeIds)
        {
            DiscountAmountSuspend = discountAmountSuspend;
            SPSuspensionTimeIds = sPSuspensionTimeIds;
        }
    }
}
