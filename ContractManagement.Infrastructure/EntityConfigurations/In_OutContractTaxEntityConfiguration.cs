using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class InContractTaxEntityConfiguration : IEntityTypeConfiguration<InContractTax>
    {
        public void Configure(EntityTypeBuilder<InContractTax> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder.Ignore(p => p.DomainEvents);

            //entityTypeBuilder.HasOne<InContract>()
            //    .WithMany()
            //    .HasForeignKey(p => p.InContractId)
            //    .IsRequired(false);
        }
    }
}