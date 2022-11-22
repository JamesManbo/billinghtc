using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ServicePackagePriceEntityConfiguration : IEntityTypeConfiguration<ServicePackagePrice>
    {
        public void Configure(EntityTypeBuilder<ServicePackagePrice> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne<ServicePackage>()
                    .WithMany()
                    .HasForeignKey(p => p.ChannelId);
        }
    }
}
