using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class OutContractServicePackageTaxEntityConfiguration : IEntityTypeConfiguration<OutContractServicePackageTax>
    {
        public void Configure(EntityTypeBuilder<OutContractServicePackageTax> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ot => new { ot.OutContractServicePackageId, ot.TaxCategoryId });

            entityTypeBuilder.HasOne(e => e.OutContractServicePackage)
                .WithMany(e => e.TaxValues)
                .HasForeignKey(p => p.OutContractServicePackageId)
                .IsRequired(true);

            entityTypeBuilder.HasOne(e => e.TaxCategory)
                .WithMany()
                .HasForeignKey(p => p.TaxCategoryId)
                .IsRequired(true);
        }
    }
}
