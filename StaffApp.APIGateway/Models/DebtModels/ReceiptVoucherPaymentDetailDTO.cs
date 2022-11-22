using DebtManagement.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.DebtModels
{
    public class ReceiptVoucherPaymentDetailDTO
    {
        public int Id { get; set; }
        public string ReceiptVoucherId { get; set; }
        public string CashierUserId { get; set; }
        public string DebtHistoryId { get; set; }
        public DateTime IssuedDate { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public Money PaidAmount { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int IssuedMonth => this.IssuedDate.Month;
    }
}
