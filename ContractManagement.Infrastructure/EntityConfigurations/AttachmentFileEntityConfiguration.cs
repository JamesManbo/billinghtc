using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class AttachmentFileEntityConfiguration : IEntityTypeConfiguration<AttachmentFile>
    {
        public void Configure(EntityTypeBuilder<AttachmentFile> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder.Ignore(p => p.DomainEvents);

            entityTypeBuilder.HasOne<OutContract>()
                .WithMany()
                .HasForeignKey(p => p.OutContractId)
                .IsRequired(false);

            entityTypeBuilder.HasOne<InContract>()
                .WithMany()
                .HasForeignKey(p => p.InContractId)
                .IsRequired(false);

            entityTypeBuilder.HasOne<Transaction>()
                .WithMany()
                .HasForeignKey(p => p.TransactionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}