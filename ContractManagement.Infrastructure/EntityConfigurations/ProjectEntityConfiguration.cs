using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasKey(c => c.Id);

            builder.HasIndex(b => b.IdentityGuid)
                .IsUnique();

            builder.HasOne<MarketArea>()
                    .WithMany()
                    .HasForeignKey(p => p.MarketAreaId);

            builder
               .HasMany<ProjectTechnician>()
               .WithOne()
               .HasForeignKey(sc => sc.ProjectId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
