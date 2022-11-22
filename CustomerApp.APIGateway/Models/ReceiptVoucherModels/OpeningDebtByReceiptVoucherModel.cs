using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class OpeningDebtByReceiptVoucherModel
    {
        public string Id { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IssuedMonth => IssuedDate.Month;
        public PaymentStatus Status { get; set; }
        public string ReceiptVoucherId { get; set; }
        public string ReceiptVoucherCode { get; set; }
        public string ReceiptVoucherContent { get; set; }
        public string SubstituteVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public MoneyDTO OpeningTargetDebtTotal { get; set; }
        public MoneyDTO OpeningCashierDebtTotal { get; set; }
        public int NumberOfPaymentDetails { get; set; }
        public string CurrencyUnitCode { get; set; }
        public IEnumerable<ReceiptVoucherPaymentDetailDTO> PaymentDetails { get; set; }
        
    }

    public enum PaymentStatus
    {
        Assigned = 0,
        CollectionOnBeHalf = 1,
        Accounted = 8
    }
}
