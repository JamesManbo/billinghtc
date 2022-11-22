using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ServicesEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne<ServiceGroup>()
                .WithMany()
                .HasForeignKey(p => p.GroupId)
                .IsRequired(false);
        }
    }
}