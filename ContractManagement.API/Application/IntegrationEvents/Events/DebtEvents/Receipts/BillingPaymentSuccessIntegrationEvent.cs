using ContractManagement.Domain.Models.ReceiptVouchers;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents
{
    public class BillingPaymentSuccessIntegrationEvent : IntegrationEvent
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public int OutContractId { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucherDTO> Promotions { get; set; }
    }
}
