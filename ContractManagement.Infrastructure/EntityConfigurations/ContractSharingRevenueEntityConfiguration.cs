using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractSharingRevenueEntityConfiguration : IEntityTypeConfiguration<ContractSharingRevenueLine>
    {
        public void Configure(EntityTypeBuilder<ContractSharingRevenueLine> builder)
        {
            builder.Ignore(c => c.DomainEvents);

            builder.HasOne<InContract>()
                .WithMany(c => c.ContractSharingRevenues)
                .HasForeignKey(p => p.InContractId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<OutContract>()
                .WithMany(c => c.ContractSharingRevenues)
                .HasForeignKey(p => p.OutContractId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<OutContractServicePackage>()
                .WithMany()
                .HasForeignKey(p => p.InServiceChannelId)
                .IsRequired(false);

            builder.HasOne<OutContractServicePackage>()
                .WithMany()
                .HasForeignKey(p => p.OutServiceChannelId)
                .IsRequired(false);

            builder.HasMany<SharingRevenueLineDetail>()
                .WithOne()
                .HasForeignKey(p => p.SharingLineId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}