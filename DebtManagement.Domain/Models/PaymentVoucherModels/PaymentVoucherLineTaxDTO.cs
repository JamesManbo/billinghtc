using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PaymentVoucherModels
{
    public class PaymentVoucherLineTaxDTO
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int VoucherLineId { get; set; }
    }
}
