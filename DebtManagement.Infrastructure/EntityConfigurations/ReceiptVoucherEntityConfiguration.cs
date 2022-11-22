using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class ReceiptVoucherEntityConfiguration : IEntityTypeConfiguration<ReceiptVoucher>
    {
        public void Configure(EntityTypeBuilder<ReceiptVoucher> builder)
        {
            builder.Ignore(b => b.DomainEvents);
            builder.Ignore(b => b.ValidReceiptVoucherDetails);

            builder.HasIndex(e => e.VoucherCode)
                .IsUnique();

            builder.HasIndex(e => e.IdentityGuid)
                .IsUnique();

            builder.HasIndex(e => e.CreatedDate);
            builder.HasIndex(e => e.IssuedDate);
            builder.HasIndex(e => e.TargetId);
            builder.HasIndex(e => e.StatusId);
            builder.HasIndex(e => e.GrandTotal);
            builder.HasIndex(e => e.PaidTotal);
            builder.HasIndex(e => e.ContractCode);
            builder.HasIndex(e => e.OutContractId);
            builder.HasIndex(e => e.OrganizationUnitId);
            builder.HasIndex(e => e.MarketAreaId);
            builder.HasIndex(e => e.OrganizationPath);

            builder.HasOne<VoucherTarget>()
                .WithMany()
                .HasForeignKey(e => e.TargetId)
                .IsRequired(false);

            builder.OwnsOne(b => b.Discount, a => a.WithOwner());
            builder.OwnsOne(b => b.Payment, a => a.WithOwner());

            builder.HasMany(e => e.ReceiptVoucherDetails)
                .WithOne(e => e.ReceiptVoucher)
                .HasForeignKey(c => c.ReceiptVoucherId)
                .IsRequired(false);

            builder.HasMany(e => e.DebtHistories)
                .WithOne()
                .HasForeignKey(c => c.ReceiptVoucherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.AttachmentFiles)
                .WithOne()
                .HasForeignKey(s => s.ReceiptVoucherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.DebtHistories)
                .WithOne()
                .HasForeignKey(s => s.ReceiptVoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.OpeningDebtHistories)
                .WithOne()
                .HasForeignKey(c => c.SubstituteVoucherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);
        }
    }
}
