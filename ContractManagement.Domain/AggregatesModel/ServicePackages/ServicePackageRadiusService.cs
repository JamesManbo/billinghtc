using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ServicePackageRadiusServices")]
    public class ServicePackageRadiusService : Entity
    {
        public int ServicePackageId { get; set; }
        public int RadiusServerId { get; set; }
        public int RadiusServiceId { get; set; }
        public virtual ServicePackage ServicePackage { get; set; }
        public void Binding(ServicePackageRadiusServiceCommand command)
        {
            RadiusServiceId = command.RadiusServiceId;
            ServicePackageId = command.ServicePackageId;
            RadiusServerId = command.RadiusServerId;
        }
    }
}
