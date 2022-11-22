using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using OrganizationUnit.Domain.AggregateModels.PictureAggregate;
using OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate;

namespace OrganizationUnit.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder
                .HasMany<UserRole>()
                .WithOne()
                .HasForeignKey(sc => sc.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            //entityTypeBuilder.HasOne(o => o.JobPosition)
            //    .WithMany(m => m.Users)
            //    .HasForeignKey(sc => sc.JobPositionId);

            entityTypeBuilder
                .HasOne<OrganizationUnit.Domain.AggregateModels.PictureAggregate.Picture>()
                .WithMany()
                .HasForeignKey(u => u.AvatarId);

            entityTypeBuilder
               .HasMany(u => u.UserBankAccounts)
               .WithOne()
               .HasForeignKey(uba => uba.UserId);

            entityTypeBuilder.HasMany(o => o.ContactInfos)
                .WithOne()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
