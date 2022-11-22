using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    internal class OrganizationUnitUsersEntityTypeConfiguration : IEntityTypeConfiguration<OrganizationUnitUser
        >
    {
        public void Configure(EntityTypeBuilder<OrganizationUnitUser> builder)
        {
            builder.HasKey(b => new
            {
                b.OrganizationUnitId,
                b.UserId
            });

            builder.HasOne<User>()
                .WithMany(u => u.OrganizationUnitUsers)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>()
                .WithMany(o => o.OrganizationUnitUsers)
                .HasForeignKey(b => b.OrganizationUnitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
