using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ClearingEntityConfiguration : IEntityTypeConfiguration<Clearing>
    {
        public void Configure(EntityTypeBuilder<Clearing> builder)
        {
            builder.Ignore(b => b.DomainEvents);
            builder.OwnsOne(b => b.Payment, a => a.WithOwner());

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);

            builder.HasOne<VoucherTarget>()
                .WithMany()
                .HasForeignKey(e => e.TargetId);

            builder.HasMany(c => c.ReceiptVouchers)
                .WithOne()
                .HasForeignKey(e => e.ClearingId)
                .IsRequired(false);

            builder.HasMany(c => c.PaymentVouchers)
                .WithOne()
                .HasForeignKey(e => e.ClearingId)
                .IsRequired(false);
        }
    }
}
