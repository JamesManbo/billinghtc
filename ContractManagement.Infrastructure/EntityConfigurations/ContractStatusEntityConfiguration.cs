using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractStatusEntityConfiguration : IEntityTypeConfiguration<ContractStatus>
    {
        public void Configure(EntityTypeBuilder<ContractStatus> builder)
        {
            builder.HasKey(ct => ct.Id);

            builder.Property(s => s.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();

        }
    }
}
