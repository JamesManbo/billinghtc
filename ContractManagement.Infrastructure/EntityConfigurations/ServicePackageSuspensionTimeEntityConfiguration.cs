using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ServicePackageSuspensionTimeEntityConfiguration : IEntityTypeConfiguration<ServicePackageSuspensionTime>
    {
        public void Configure(EntityTypeBuilder<ServicePackageSuspensionTime> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasOne(c => c.Channel)
                .WithMany()
                .HasForeignKey(c => c.OutContractServicePackageId);
        }
    }
}
