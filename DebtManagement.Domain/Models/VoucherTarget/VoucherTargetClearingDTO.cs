using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.VoucherTarget
{
   public class VoucherTargetClearingDTO : BaseDTO
    {
        public string TargetFullName { get; set; }
        public string TargetCode { get; set; }
        public decimal TotalReceiptVouchers { get; set; }
        public decimal TotalPaymentVouchers { get; set; }
    }
}
