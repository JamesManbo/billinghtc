using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
  
    public class PromotionTypeEntityConfiguration : IEntityTypeConfiguration<PromotionType>
    {
        public void Configure(EntityTypeBuilder<PromotionType> builder)
        {

        }
    }
}
