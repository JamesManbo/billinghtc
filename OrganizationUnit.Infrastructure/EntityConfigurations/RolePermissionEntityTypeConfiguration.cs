using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class RolePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);
        }
    }
}
