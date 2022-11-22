using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder
                .HasMany<UserRole>()
                .WithOne()
                .HasForeignKey(sc => sc.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entityTypeBuilder
                .HasMany<RolePermission>()
                .WithOne()
                .HasForeignKey(sc => sc.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
