using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.RadiusAndBrasCommand
{
    public class ServicePackageRadiusServiceCommand
    {
        public int Id { get; set; }
        public int ServicePackageId { get; set; }
        public int RadiusServerId { get; set; }
        public string RadiusServerName { get; set; }
        public int RadiusServiceId { get; set; }
    }
}
