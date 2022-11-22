using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractEquipmentEntityConfiguration : IEntityTypeConfiguration<ContractEquipment>
    {
        public void Configure(EntityTypeBuilder<ContractEquipment> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasOne<TransactionEquipment>()
               .WithOne()
               .HasForeignKey<ContractEquipment>(sp => sp.TransactionEquipmentId);
        }
    }
}
