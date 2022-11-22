using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("PaymentVoucherLineTaxes")]
    public class PaymentVoucherLineTax : Entity
    {
        public PaymentVoucherLineTax()
        {
        }

        public PaymentVoucherLineTax(CreatePaymentVoucherLineTaxCommand command)
        {
            IdentityGuid = string.IsNullOrEmpty(command.IdentityGuid) 
                ? Guid.NewGuid().ToString()
                : command.IdentityGuid;

            CreatedBy = command.CreatedBy;
            CreatedDate = DateTime.Now;
            VoucherLineId = command.VoucherLineId;
            TaxCode = command.TaxCode;
            TaxName = command.TaxName;
            TaxValue = command.TaxValue;
            IsAutomaticGenerate = command.IsAutomaticGenerate;
        }

        public int? VoucherLineId { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public bool IsAutomaticGenerate { get; set; }
        [IgnoreDataMember]
        public virtual PaymentVoucherDetail PaymentVoucherLine { get; set; }
    }
}
