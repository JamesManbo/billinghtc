using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class VoucherTargetEntityConfiguration : IEntityTypeConfiguration<VoucherTarget>
    {
        public void Configure(EntityTypeBuilder<VoucherTarget> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasIndex(e => e.IdentityGuid)
                .IsUnique();

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);

            builder.Property(c => c.CurrentDebt)
                .HasDefaultValue(0);

            builder.HasMany<VoucherTargetProperty>()
                .WithOne()
                .HasForeignKey(p => p.TargetId)
                .IsRequired(false);
        }
    }
}
