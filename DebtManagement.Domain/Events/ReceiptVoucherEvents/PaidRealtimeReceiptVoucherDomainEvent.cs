using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ReceiptVoucherEvents
{
    public class PaidRealtimeReceiptVoucherDomainEvent: INotification
    {
        public IEnumerable<PaidRealtimeServicePackage> PaidRealtimeServicePackages { get; set; }
    }

    public class PaidRealtimeServicePackage
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OutContractServicePackageId { get; set; }
    }
}
