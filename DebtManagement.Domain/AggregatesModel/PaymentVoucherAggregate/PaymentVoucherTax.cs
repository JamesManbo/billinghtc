using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVoucherTaxes")]
    public class PaymentVoucherTax: Entity
    {
        public PaymentVoucherTax()
        {
        }

        public PaymentVoucherTax(CreatePaymentVoucherTaxCommand command)
        {
            VoucherId = command.VoucherId;
            TaxName = command.TaxName;
            TaxCode = command.TaxCode;
            TaxValue = command.TaxValue;
        }

        public int VoucherId { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public virtual PaymentVoucher PaymentVoucher { get; set; }
    }
}
