using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class OrganizationUnitEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ct => ct.Id);
        }
    }
}
