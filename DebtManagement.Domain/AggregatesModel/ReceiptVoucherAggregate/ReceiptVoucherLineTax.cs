using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherLineTaxes")]
    public class ReceiptVoucherLineTax : Entity
    {
        public ReceiptVoucherLineTax()
        {
        }

        public ReceiptVoucherLineTax(CreateReceiptVoucherLineTaxCommand command)
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
        public virtual ReceiptVoucherDetail ReceiptVoucherLine { get; set; }
    }
}
