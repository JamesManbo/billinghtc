using System.Collections.Generic;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Commands.ServicePackageCommand
{
    public class CUServicePackageCommand : IRequest<ActionResponse<ServicePackageDTO>>
    {
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int PackageId { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public int? ParentId { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public int InternationalBandwidthUomId { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public int DomesticBandwidthUomId { get; set; }
        public decimal Price { get; set; }
        public string CreateBy { get; set; }
        public List<ServicePackagePriceDTO> ListProjectPrice { get; set; }
        public List<ServicePackagePriceDTO> ListDelProjectPrices { get; set; }
        public List<ServicePackageRadiusServiceCommand> ServicePackageRadiusServices { get; set; }
        public List<ServicePackagePriceDTO> ListPriceByCurrencyUnit { get; set; }
    }
}
