using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ContractEvents
{
    public class BillingPaymentSuccessEvent : INotification
    {
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public int OutContractId { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public List<ReceiptVoucherDetail> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucher> Promotions { get; set; }
    }
}
