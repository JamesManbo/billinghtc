using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class OutContractServicePackageClearingEntityConfiguration : IEntityTypeConfiguration<OutContractServicePackageClearing>
    {
        public void Configure(EntityTypeBuilder<OutContractServicePackageClearing> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne<OutContractServicePackage>()
                    .WithMany()
                    .HasForeignKey(p => p.OutContractServicePackageId);
        }
    }
}
