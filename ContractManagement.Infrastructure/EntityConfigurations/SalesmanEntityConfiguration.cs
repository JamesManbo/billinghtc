using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class SalesmanEntityConfiguration : IEntityTypeConfiguration<Salesman>
    {
        public void Configure(EntityTypeBuilder<Salesman> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasIndex(b => b.IdentityGuid)
                .IsUnique();
        }
    }
}
