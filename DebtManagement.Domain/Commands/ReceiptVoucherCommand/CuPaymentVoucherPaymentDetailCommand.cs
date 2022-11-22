using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CuPaymentVoucherPaymentDetailCommand
    {
        public int Id { get; set; }
        public int PaymentVoucherId { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentTurn { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal? PaidAmount { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? PaymentDate { get; set; }

    }
}
