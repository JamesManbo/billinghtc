using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class BrasInformationEntityConfigurations : IEntityTypeConfiguration<BrasInformation>
    {
        public void Configure(EntityTypeBuilder<BrasInformation> builder)
        {
            builder.Ignore(e => e.DomainEvents);
        }
    }
}
