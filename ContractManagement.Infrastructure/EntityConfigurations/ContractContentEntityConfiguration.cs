using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractContentEntityConfiguration : IEntityTypeConfiguration<ContractContent>
    {
        public void Configure(EntityTypeBuilder<ContractContent> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder.Ignore(p => p.DomainEvents);

            entityTypeBuilder
                .HasOne(e => e.DigitalSignature)
                .WithMany()
                .HasForeignKey(u => u.DigitalSignatureId);

            entityTypeBuilder
                .HasOne(e => e.ContractFormSignature)
                .WithMany()
                .HasForeignKey(u => u.ContractFormSignatureId)
                .IsRequired(false);

            entityTypeBuilder
                .HasOne(ad => ad.OutContract)
                .WithOne(s => s.ContractContent)
                .HasForeignKey<ContractContent>(s => s.OutContractId)
                .IsRequired(false);

            entityTypeBuilder
                .HasOne(ad => ad.InContract)
                .WithOne(s => s.ContractContent)
                .HasForeignKey<ContractContent>(s => s.InContractId)
                .IsRequired(false);
        }
    }
}
