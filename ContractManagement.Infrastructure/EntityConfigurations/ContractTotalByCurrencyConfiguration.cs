using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractTotalByCurrencyConfiguration : IEntityTypeConfiguration<ContractTotalByCurrency>
    {
        public void Configure(EntityTypeBuilder<ContractTotalByCurrency> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasOne<OutContract>()
                .WithMany(c => c.ContractTotalByCurrencies)
                .HasForeignKey(c => c.OutContractId)
                .IsRequired(false);

            builder.HasOne<InContract>()
                .WithMany(c => c.ContractTotalByCurrencies)
                .HasForeignKey(c => c.InContractId)
                .IsRequired(false);
        }
    }
}