using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class OutContractEntityConfiguration : IEntityTypeConfiguration<OutContract>
    {
        public void Configure(EntityTypeBuilder<OutContract> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.Ignore(b => b.ActiveServicePackages);

            builder.HasIndex(e => e.IdentityGuid)
                .IsUnique();

            builder.HasIndex(e => e.ContractCode)
                .IsUnique(true);

            builder.OwnsOne(o => o.Payment, a => a.WithOwner());
            builder.OwnsOne(o => o.TimeLine, a => a.WithOwner());

            builder.HasMany(sp => sp.ServicePackages)
                .WithOne(c => c.OutContract)
                .HasForeignKey(sp => sp.OutContractId);

            builder.HasMany(o => o.ContactInfos)
                .WithOne()
                .HasForeignKey(o => o.OutContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Transactions)
                .WithOne()
                .HasForeignKey(s => s.OutContractId)
                .IsRequired(false);
        }
    }
}