using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.PackageModels
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string BandwidthLabel { get; set; }
        public int InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public int DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public MoneyDTO Price { get; set; }
        public string Description { get; set; }
    }
}
