using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class CreatePaymentVoucherTaxCommand
    {
        public int VoucherId { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
