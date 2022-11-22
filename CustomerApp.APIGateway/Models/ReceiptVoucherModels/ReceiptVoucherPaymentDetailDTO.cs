using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherPaymentDetailDTO
    {
        public string Id { get; set; }

        public string ReceiptVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string DebtHistoryId { get; set; }
        public DateTime IssuedDate { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public MoneyDTO PaidAmount { get; set; }
        public bool IsActive { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int IssuedMonth => this.IssuedDate.Month;
    }
}
