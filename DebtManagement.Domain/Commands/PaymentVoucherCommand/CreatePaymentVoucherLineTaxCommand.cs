using System;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class CreatePaymentVoucherLineTaxCommand
    {
        public string IdentityGuid { get; set; }
        public int VoucherLineId { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public string CreatedBy { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        public DateTime CreatedDate { get; set; }

        public CreatePaymentVoucherLineTaxCommand ShallowCopy()
        {
            return (CreatePaymentVoucherLineTaxCommand)this.MemberwiseClone();
        }
    }
}
