using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ContractFormEntityConfiguration : IEntityTypeConfiguration<ContractForm>
    {
        public void Configure(EntityTypeBuilder<ContractForm> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder.Ignore(p => p.DomainEvents);

            entityTypeBuilder
                .HasOne(e => e.DigitalSignature)
                .WithMany()
                .HasForeignKey(u => u.DigitalSignatureId)
                .IsRequired(false);
        }
    }
}
