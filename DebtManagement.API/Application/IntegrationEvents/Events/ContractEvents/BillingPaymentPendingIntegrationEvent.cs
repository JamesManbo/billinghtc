using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models;
using EventBus.Events;
using System.Collections.Generic;

namespace DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class BillingPaymentPendingIntegrationEvent : IntegrationEvent
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucher> Promotions { get; set; }
        public BillingPaymentPendingIntegrationEvent()
        {
            VoucherDetails = new List<ReceiptVoucherDetailDTO>();
            Promotions = new List<PromotionForReceiptVoucher>();
        }
    }
}
