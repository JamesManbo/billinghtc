using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ServicePackageDTO
    {
        public ServicePackageDTO()
        {
            ServicePackageRadiusServices = new List<ServicePackageRadiusServiceDTO>();
            //ListPrice = new List<ServicePackagePriceDTO>();
        }
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentCode { get; set; }
        public string ServiceName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthLabel { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public int? InternationalBandwidthUomId { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthLabel { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public int? DomesticBandwidthUomId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasStartAndEndPoint{ get; set; }
        public string StateLabel => !IsActive ? "Đã khóa" : "Đang hoạt động";
        public string CreateBy { get; set; }
        public List<ServicePackageRadiusServiceDTO> ServicePackageRadiusServices { get; set; }

        public byte IsDefaultSLAByServiceId { get; set; }
        //public List<ServicePackagePriceDTO> ListPrice { get; set; }
    }
}
