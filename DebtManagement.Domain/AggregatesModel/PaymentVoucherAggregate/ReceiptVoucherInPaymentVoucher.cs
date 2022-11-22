using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("ReceiptVoucherInPaymentVoucher")]
    public class ReceiptVoucherInPaymentVoucher : Entity
    {
        public int PaymentVoucherId { get; set; }
        public int ReceiptVoucherId { get; set; }
    }
}
