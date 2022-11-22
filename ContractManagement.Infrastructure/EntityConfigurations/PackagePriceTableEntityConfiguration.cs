using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class PackagePriceTableEntityConfiguration : IEntityTypeConfiguration<ChannelPriceBusTable>
    {
        public void Configure(EntityTypeBuilder<ChannelPriceBusTable> builder)
        {   
            builder.Ignore(c => c.DomainEvents);

            builder.HasOne<OutContractServicePackage>()
                .WithMany(e => e.PriceBusTables)
                .HasForeignKey(c => c.ChannelId)
                .IsRequired(false);
        }
    }
}
