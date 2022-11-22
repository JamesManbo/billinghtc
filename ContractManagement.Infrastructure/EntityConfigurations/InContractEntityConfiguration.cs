using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.SalesmanAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class InContractEntityConfiguration : IEntityTypeConfiguration<InContract>
    {
        public void Configure(EntityTypeBuilder<InContract> builder)
        {
            builder.Ignore(b => b.ActiveServicePackages);

            builder.HasIndex(e => e.IdentityGuid)
                .IsUnique(true);

            builder.HasIndex(e => e.ContractCode)
                .IsUnique(true);

            builder.Ignore(b => b.DomainEvents);

            builder.OwnsOne(o => o.TimeLine, a => a.WithOwner());
            builder.OwnsOne(o => o.Payment, a => a.WithOwner());

            builder.HasMany(o => o.ContactInfos)
                .WithOne()
                .HasForeignKey(o => o.InContractId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(o => o.InContractServices)
            //    .WithOne()
            //    .HasForeignKey(o => o.InContractId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.ServicePackages)
                .WithOne()
                .HasForeignKey(o => o.InContractId)
                .IsRequired(false);

            builder.HasMany<Transaction>()
                .WithOne()
                .HasForeignKey(s => s.InContractId)
                .IsRequired(false);
        }
    }
}