using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class UserBankAccountEntityConfiguration : IEntityTypeConfiguration<UserBankAccount>
    {
        public void Configure(EntityTypeBuilder<UserBankAccount> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}
