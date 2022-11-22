using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class AttachmentFileEntityConfiguration : IEntityTypeConfiguration<AttachmentFile>
    {
        public void Configure(EntityTypeBuilder<AttachmentFile> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);

            builder.HasOne<ReceiptVoucherDetail>()
                .WithMany()
                .HasForeignKey(p => p.ReceiptVoucherDetailId)
                .IsRequired(false);

            builder.HasOne<Clearing>()
                .WithMany()
                .HasForeignKey(c => c.ClearingVoucherId)
                .IsRequired(false);
        }
    }
}
