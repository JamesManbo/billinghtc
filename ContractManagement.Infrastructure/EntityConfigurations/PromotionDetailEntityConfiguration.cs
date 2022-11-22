using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure
{
    public class PromotionDetailEntityConfiguration : IEntityTypeConfiguration<PromotionDetail>
    {
        public void Configure(EntityTypeBuilder<PromotionDetail> builder)
        {

        }
    }
}