using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionPriceBusTableEntityConfiguration : IEntityTypeConfiguration<TransactionPriceBusTable>
    {
        public void Configure(EntityTypeBuilder<TransactionPriceBusTable> builder)
        {
            builder.Ignore(c => c.DomainEvents);

            builder.HasOne<TransactionServicePackage>()
                .WithMany(e => e.PriceBusTables)
                .HasForeignKey(c => c.TransactionServicePackageId)
                .IsRequired(false);
        }
    }
}
