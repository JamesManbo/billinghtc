using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents
{
    public class NextBillingToReceiptVoucherIntegrationEvent: IntegrationEvent
    {
        public int OutContractServicePackageId { get; set; }
        public DateTime OldNextBilling { get; set; }
        public DateTime NewNextBilling { get; set; }
        public int PaymentPeriod { get; set; }

        public NextBillingToReceiptVoucherIntegrationEvent(int outContractServicePackageId, DateTime oldNextBilling, DateTime newNextBilling, int paymentPeriod)
        {
            OutContractServicePackageId = outContractServicePackageId;
            OldNextBilling = oldNextBilling;
            NewNextBilling = newNextBilling;
            PaymentPeriod = paymentPeriod;
        }
    }
}
