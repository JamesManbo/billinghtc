using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ChannelPriceBusTableEntityConfiguration : IEntityTypeConfiguration<ChannelPriceBusTable>
    {
        public void Configure(EntityTypeBuilder<ChannelPriceBusTable> builder)
        {
            builder.Ignore(c => c.DomainEvents);
            builder.HasOne<ReceiptVoucherDetail>()
                .WithMany(c => c.PriceBusTables)
                .HasForeignKey(c => c.ReceiptVoucherLineId)
                .IsRequired(true);
        }
    }
}
