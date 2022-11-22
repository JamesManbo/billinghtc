using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class UnitOfMeasurementEntityConfiguation : IEntityTypeConfiguration<UnitOfMeasurement>
    {
        public void Configure(EntityTypeBuilder<UnitOfMeasurement> unitOfMeasurementConfiguration)
        {
            unitOfMeasurementConfiguration.Ignore(e => e.DomainEvents);

            unitOfMeasurementConfiguration.HasIndex(b => b.Id)
                .IsUnique();
        }
    }
}
