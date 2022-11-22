using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ReceiptVoucherDetailEntityConfiguration : IEntityTypeConfiguration<ReceiptVoucherDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptVoucherDetail> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasIndex(c => c.IdentityGuid)
                .IsUnique();

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);

            builder.HasMany(e => e.ReceiptVoucherLineTaxes)
                .WithOne(e => e.ReceiptVoucherLine)
                .HasForeignKey(c => c.VoucherLineId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
