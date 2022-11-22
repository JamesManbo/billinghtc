using System;
using System.Collections.Generic;

namespace ContractManagement.Domain.Models
{
    public class ServicePackageSimpleDTO
    {
        public ServicePackageSimpleDTO()
        {
        }

        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string DisplayName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
    }
}
