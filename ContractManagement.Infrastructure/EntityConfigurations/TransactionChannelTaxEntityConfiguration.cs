using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionChannelTaxEntityConfiguration : IEntityTypeConfiguration<TransactionChannelTax>
    {
        public void Configure(EntityTypeBuilder<TransactionChannelTax> builder)
        {
            builder.HasKey(t => new
            {
                t.TaxCategoryId,
                t.TransactionServicePackageId
            });

            builder.HasOne(sp => sp.TaxCategory)
               .WithMany()
               .HasForeignKey(sp => sp.TaxCategoryId)
               .IsRequired(true);

            builder.HasOne<TransactionServicePackage>()
               .WithMany(ts => ts.TaxValues)
               .HasForeignKey(sp => sp.TransactionServicePackageId)
               .IsRequired(true);
        }
    }
}
