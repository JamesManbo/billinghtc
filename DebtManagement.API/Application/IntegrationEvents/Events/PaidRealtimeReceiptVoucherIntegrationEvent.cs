using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class PaidRealtimeReceiptVoucherIntegrationEvent : IntegrationEvent
    {
        public List<PaidRealtimeServicePackage> PaidRealtimeServicePackages { get; set; }

        public PaidRealtimeReceiptVoucherIntegrationEvent()
        {
        }
    }

    public class PaidRealtimeServicePackage
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OutContractServicePackageId { get; set; }
    }
}
