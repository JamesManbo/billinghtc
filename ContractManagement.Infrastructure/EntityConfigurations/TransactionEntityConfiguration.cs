using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasMany(sp => sp.TransactionServicePackages)
               .WithOne()
               .HasForeignKey(sp => sp.TransactionId);

            builder.HasMany(s => s.AttachmentFiles)
                .WithOne()
                .HasForeignKey(s => s.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<TransactionReason>()
               .WithMany()
               .HasForeignKey(sp => sp.ReasonType);
        }
    }
}
