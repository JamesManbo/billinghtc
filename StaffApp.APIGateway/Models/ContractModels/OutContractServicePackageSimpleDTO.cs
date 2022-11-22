using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class OutContractServicePackageSimpleDTO
    {
        public int Id { get; set; }
        public string ContractCode { get; set; }
        public int ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public string CId { get; set; }
        public string BandwidthLabel { get; set; }
        public MoneyDTO PackagePrice { get; set; }
        public MoneyDTO SpPackagePrice { get; set; }
        public MoneyDTO EpPackagePrice { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string Text { get; set; }
    }
}
