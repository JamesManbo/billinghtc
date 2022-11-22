using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class EquipmentPictureEntityConfiguation : IEntityTypeConfiguration<EquipmentPicture>
    {
        public void Configure(EntityTypeBuilder<EquipmentPicture> entityTypeBuilder)
        {
            //entityTypeBuilder.HasKey(k => k.Id);
        }
    }
}
