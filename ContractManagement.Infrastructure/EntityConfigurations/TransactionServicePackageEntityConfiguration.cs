using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionServicePackageEntityConfiguration : IEntityTypeConfiguration<TransactionServicePackage>
    {
        public void Configure(EntityTypeBuilder<TransactionServicePackage> builder)
        {
            builder.Ignore(o => o.DomainEvents);
            builder.HasOne(b => b.PaymentTarget)
                .WithMany()
                .HasForeignKey(c => c.PaymentTargetId)
                .IsRequired(true);

            builder.OwnsOne(o => o.TimeLine, a => a.WithOwner());

            builder.HasOne(e => e.StartPoint)
               .WithMany()
               .HasForeignKey(c => c.StartPointChannelId)
               .IsRequired(false);

            builder.HasOne(e => e.EndPoint)
               .WithMany()
               .HasForeignKey(c => c.EndPointChannelId)
               .IsRequired(false);

            builder.HasMany(e => e.AppliedPromotions)
                .WithOne()
                .HasForeignKey(e => e.TransactionServicePackageId);

            builder.HasMany(e => e.ServiceLevelAgreements)
                .WithOne()
                .HasForeignKey(e => e.TransactionServicePackageId)
                .IsRequired(false);
        }
    }
}
