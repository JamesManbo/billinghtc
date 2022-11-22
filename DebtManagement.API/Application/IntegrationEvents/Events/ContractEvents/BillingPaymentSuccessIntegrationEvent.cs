using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.Events.ContractEvents
{
    public class BillingPaymentSuccessIntegrationEvent : IntegrationEvent
    {
        public BillingPaymentSuccessIntegrationEvent()
        {
            VoucherDetails = new List<ReceiptVoucherDetailDTO>();
            Promotions = new List<PromotionForReceiptVoucher>();
        }

        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public int OutContractId { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public List<ReceiptVoucherDetailDTO> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucher> Promotions { get; set; }
    }
}
