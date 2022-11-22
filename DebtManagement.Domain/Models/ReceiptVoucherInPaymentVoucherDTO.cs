using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ReceiptVoucherInPaymentVoucherDTO : VoucherBaseDTO
    {
        public int PaymentVoucherId { get; set; }
    }
}
