using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;

namespace DebtManagement.Infrastructure.EntityConfigurations
{
    public class PaymentVoucherDetailEntityConfiguration : IEntityTypeConfiguration<PaymentVoucherDetail>
    {
        public void Configure(EntityTypeBuilder<PaymentVoucherDetail> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasIndex(e => e.OrganizationPath)
                .IsUnique(false);
        }
    }
}
