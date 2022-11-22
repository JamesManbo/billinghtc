using ContractManagement.Domain.Models.PaymentVouchers;
using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments
{
    public class PaymentVoucherCreatedIntegrationEvent : IntegrationEvent, IRequest
    {
        public int TargetId { get; set; }
        public decimal GrandTotal { get; set; }
        public int InContractId { get; set; }
        public List<PaymentVoucherDetailDTO> VoucherDetails { get; set; }
        public DateTime? NextBillingDate { get; set; }
    }
}
