using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class OutputChannelPointEntityConfiguration : IEntityTypeConfiguration<OutputChannelPoint>
    {
        public void Configure(EntityTypeBuilder<OutputChannelPoint> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.OwnsOne(b => b.InstallationAddress, a => a.WithOwner());

            builder.HasMany(c => c.Equipments)
                .WithOne()
                .HasForeignKey(c => c.OutputChannelPointId);
        }
    }
}
