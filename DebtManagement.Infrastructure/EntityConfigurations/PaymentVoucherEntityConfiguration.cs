using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class PaymentVoucherEntityConfiguration : IEntityTypeConfiguration<PaymentVoucher>
    {
        public void Configure(EntityTypeBuilder<PaymentVoucher> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasIndex(e => e.VoucherCode)
                .IsUnique();

            builder.OwnsOne(b => b.Discount, a => a.WithOwner());
            builder.OwnsOne(b => b.Payment, a => a.WithOwner());

            builder.HasOne<VoucherTarget>()
                .WithMany()
                .HasForeignKey(e => e.TargetId);

            builder.HasMany(e => e.PaymentVoucherDetails)
                .WithOne()
                .HasForeignKey(c => c.PaymentVoucherId);

            builder.HasMany(e => e.PaymentVoucherTaxes)
                .WithOne(e => e.PaymentVoucher)
                .HasForeignKey(c => c.VoucherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.PaymentDetails)
                .WithOne()
                .HasForeignKey(c => c.PaymentVoucherId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);
           
        }
    }
}
