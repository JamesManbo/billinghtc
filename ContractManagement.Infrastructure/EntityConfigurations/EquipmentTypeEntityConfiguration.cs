using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class EquipmentTypeEntityConfiguration : IEntityTypeConfiguration<EquipmentType>
    {
        public void Configure(EntityTypeBuilder<EquipmentType> entityTypeBuilder)
        {
            //entityTypeBuilder.Ignore(e => e.DomainEvents);
            //entityTypeBuilder.HasKey(k => k.Id);
        }
    }
}
