using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class ConfigurationSystemEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ct => ct.Id);
        }
    }

    public class ConfigurationPersonalAccountEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount> entityTypeBuilder)
        {
            entityTypeBuilder.HasMany<User>().WithOne().HasForeignKey(u=>u.ConfigurationAccount);
        }
    }
}
