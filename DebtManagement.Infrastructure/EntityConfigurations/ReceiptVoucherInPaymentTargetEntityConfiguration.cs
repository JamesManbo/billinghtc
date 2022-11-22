using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ReceiptVoucherInPaymentTargetEntityConfiguration : IEntityTypeConfiguration<ReceiptVoucherInPaymentVoucher>
    {
        public void Configure(EntityTypeBuilder<ReceiptVoucherInPaymentVoucher> builder)
        {
           
            //builder.Ignore(e => e.ReceiptVoucherId);
        }
    }
}
