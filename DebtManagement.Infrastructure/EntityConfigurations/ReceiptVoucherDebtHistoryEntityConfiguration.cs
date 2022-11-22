using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ReceiptVoucherDebtHistoryEntityConfiguration : IEntityTypeConfiguration<ReceiptVoucherDebtHistory>
    {
        public void Configure(EntityTypeBuilder<ReceiptVoucherDebtHistory> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasMany(e => e.PaymentDetails)
                .WithOne()
                .HasForeignKey(e => e.DebtHistoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);
        }
    }
}
