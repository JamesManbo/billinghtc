using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractorEntityConfiguration : IEntityTypeConfiguration<Contractor>
    {
        public void Configure(EntityTypeBuilder<Contractor> builder)
        {
            builder.HasIndex(e => e.IdentityGuid)
                .IsUnique(true);
        }
    }
}
