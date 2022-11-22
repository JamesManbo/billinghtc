using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PaymentVoucherModels
{
    public class PaymentVoucherPaymentDetailDTO : BaseDTO
    {
        public int PaymentVoucherId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal PaidAmount { get; set; }
        public int PaymentTurn { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
