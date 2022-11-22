using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class CurrencyUnitEntityConfiguration : IEntityTypeConfiguration<CurrencyUnit>
    {
        public void Configure(EntityTypeBuilder<CurrencyUnit> builder)
        {

        }
    }
}
