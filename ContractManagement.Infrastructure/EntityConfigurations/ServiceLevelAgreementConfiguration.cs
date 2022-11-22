using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure
{
    public class ServiceLevelAgreementConfiguration : IEntityTypeConfiguration<ServiceLevelAgreement>
    {
        public void Configure(EntityTypeBuilder<ServiceLevelAgreement> builder)
        {
            builder.Ignore(p => p.DomainEvents);

            builder.HasOne<OutContractServicePackage>()
                .WithMany(e => e.ServiceLevelAgreements)
                .HasForeignKey(e => e.OutContractServicePackageId)
                .IsRequired(false);
        }
    }
}