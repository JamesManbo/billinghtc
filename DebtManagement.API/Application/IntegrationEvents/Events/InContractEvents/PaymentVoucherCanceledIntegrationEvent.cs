using DebtManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events.InContractEvents
{
    public class PaymentVoucherCanceledIntegrationEvent : IntegrationEvent
    {
        public int TargetId { get; set; }
        public decimal GrandTotal { get; set; }
        public int InContractId { get; set; }
        public List<PaymentVoucherDetailDTO> VoucherDetails { get; set; }
        public DateTime? NextBillingDate { get; set; }
    }
}
