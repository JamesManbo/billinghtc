using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class BusTablePricingCalculatorEntityConfiguration : IEntityTypeConfiguration<BusTablePricingCalculator>
    {
        public void Configure(EntityTypeBuilder<BusTablePricingCalculator> builder)
        {
            builder.Ignore(c => c.DomainEvents);
            builder.HasOne<ReceiptVoucherDetail>()
                .WithMany(c => c.BusTablePricingCalculators)
                .HasForeignKey(c => c.ReceiptVoucherLineId)
                .IsRequired(true);
        }
    }
}
