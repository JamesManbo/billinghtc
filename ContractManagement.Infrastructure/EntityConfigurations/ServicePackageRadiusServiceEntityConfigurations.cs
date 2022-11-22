using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.EntityConfigurations
{
    public class ServicePackageRadiusServiceEntityConfigurations : IEntityTypeConfiguration<ServicePackageRadiusService>
    {
        public void Configure(EntityTypeBuilder<ServicePackageRadiusService> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne(e => e.ServicePackage)
                .WithMany(s => s.ServicePackageRadiusServices)
                .HasForeignKey(e => e.ServicePackageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
