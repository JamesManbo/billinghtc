using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ClearingPaymentDetailDTO : BaseDTO
    {
        public string ClearingId { get; set; }
        public string PaymentVoucherId { get; set; }
        public decimal ClearingAmount { get; set; }
    }
}
