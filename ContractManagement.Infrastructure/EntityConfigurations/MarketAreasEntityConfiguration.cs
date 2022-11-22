using ContractManagement.Domain.AggregatesModel.MarketArea;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class MarketAreasEntityConfiguration : IEntityTypeConfiguration<MarketArea>
    {
        public void Configure(EntityTypeBuilder<MarketArea> builder)
        {
            throw new NotImplementedException();
        }
    }
}
