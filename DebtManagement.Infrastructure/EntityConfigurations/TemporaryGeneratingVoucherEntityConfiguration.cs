using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class TemporaryGeneratingVoucherEntityConfiguration : IEntityTypeConfiguration<TemporaryGeneratingVoucher>
    {
        public void Configure(EntityTypeBuilder<TemporaryGeneratingVoucher> builder)
        {
            builder.HasIndex(c => c.VoucherTargetId);
            builder.HasIndex(c => c.ReceiptVoucherDetailId);
            builder.HasIndex(c => c.ReceiptVoucherId);
        }
    }
}
