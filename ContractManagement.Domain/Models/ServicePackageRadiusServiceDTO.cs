using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ServicePackageRadiusServiceDTO
    {
        public int Id { get; set; }
        public int ServicePackageId { get; set; }
        public int RadiusServerId { get; set; }
        public string RadiusServerName { get; set; }
        public int RadiusServiceId { get; set; }
        public string BillingPackageName { get; set; }
    }
}
