using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class PictureEntityConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(k => k.Id);

            entityTypeBuilder.Ignore(p => p.DomainEvents);

            //entityTypeBuilder
            //    .HasMany<EquipmentPicture>()
            //    .WithOne()
            //    .HasForeignKey(sc => sc.PictureId);
        }
    }
}
