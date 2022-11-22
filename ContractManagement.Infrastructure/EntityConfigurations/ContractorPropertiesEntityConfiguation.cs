using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractorPropertiesEntityConfiguation : IEntityTypeConfiguration<ContractorProperties>
    {
        public void Configure(EntityTypeBuilder<ContractorProperties> builder)
        {
            builder.Ignore(b => b.DomainEvents);
            builder.HasIndex(e => e.Id)
                .IsUnique();

            builder.HasOne<Contractor>()
                .WithMany()
                .HasForeignKey(p => p.ContractorId)
                .IsRequired(false);

        }
    }
}
