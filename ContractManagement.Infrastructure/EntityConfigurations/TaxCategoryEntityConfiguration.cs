using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TaxCategoryEntityConfiguration : IEntityTypeConfiguration<TaxCategory>
    {
        public void Configure(EntityTypeBuilder<TaxCategory> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasIndex(b => b.Id)
                .IsUnique();
        }
    }
}
