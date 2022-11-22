using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionChannelPointEntityConfiguration : IEntityTypeConfiguration<TransactionChannelPoint>
    {
        public void Configure(EntityTypeBuilder<TransactionChannelPoint> builder)
        {
            builder.Ignore(o => o.DomainEvents);

            builder.OwnsOne(b => b.InstallationAddress, a => a.WithOwner());

            builder.HasMany(c => c.Equipments)
                .WithOne()
                .HasForeignKey(c => c.OutputChannelPointId);
        }
    }
}
