using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ServicePackagesEntityConfiguration : IEntityTypeConfiguration<ServicePackage>
    {
        public void Configure(EntityTypeBuilder<ServicePackage> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(p => p.ServiceId)
                    .IsRequired(false);

            builder.HasOne<UnitOfMeasurement>()
                    .WithMany()
                    .HasForeignKey(p => p.DomesticBandwidthUomId);


            builder.HasOne<UnitOfMeasurement>()
                    .WithMany()
                    .HasForeignKey(p => p.InternationalBandwidthUomId);
        }
    }
}
