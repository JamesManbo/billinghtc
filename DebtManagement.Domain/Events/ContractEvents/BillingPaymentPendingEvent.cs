using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Events.ContractEvents
{
    public class BillingPaymentPendingEvent : INotification
    {
        public int TargetId { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsActiveSPST { get; set; } //Sử dụng cho ServicePackageSuspensionTimes true là đang sử dụng, false là không sử dụng
        public List<ReceiptVoucherDetail> VoucherDetails { get; set; }
        public List<PromotionForReceiptVoucher> Promotions { get; set; }
        public BillingPaymentPendingEvent()
        {
            VoucherDetails = new List<ReceiptVoucherDetail>();
            Promotions = new List<PromotionForReceiptVoucher>();
        }
    }
}
