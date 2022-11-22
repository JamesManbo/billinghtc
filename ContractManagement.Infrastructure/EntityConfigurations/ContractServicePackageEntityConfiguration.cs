using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractServicePackageEntityConfiguration : IEntityTypeConfiguration<OutContractServicePackage>
    {
        public void Configure(EntityTypeBuilder<OutContractServicePackage> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasOne(b => b.PaymentTarget)
                .WithMany()
                .HasForeignKey(c => c.PaymentTargetId)
                .IsRequired(true);

            builder.OwnsOne(b => b.TimeLine, a => a.WithOwner());

            builder.HasOne(e => e.StartPoint)
                .WithMany()
                .HasForeignKey(c => c.StartPointChannelId)
                .IsRequired(false);

            builder.HasOne(e => e.EndPoint)
                .WithMany()
                .HasForeignKey(c => c.EndPointChannelId)
                .IsRequired(true);

            builder.HasMany(e => e.AppliedPromotions)
                .WithOne()
                .HasForeignKey(e => e.OutContractServicePackageId);

            builder.HasMany(e => e.ServiceLevelAgreements)
                .WithOne()
                .HasForeignKey(e => e.OutContractServicePackageId)
                .IsRequired(false);

            builder.HasOne<TransactionServicePackage>()
               .WithOne()
               .HasForeignKey<OutContractServicePackage>(sp => sp.TransactionServicePackageId);

            builder.HasOne<FlexiblePricingType>()
                .WithMany()
                .HasForeignKey(c => c.FlexiblePricingTypeId)
                .IsRequired(true);
        }
    }
}
