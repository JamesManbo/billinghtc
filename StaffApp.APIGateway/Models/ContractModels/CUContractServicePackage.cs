using StaffApp.APIGateway.Models.BaseContractModels;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class CUContractServicePackage
    {
        public int Id { get; set; }
        public CUContractServicePackage()
        {
            //Discount = new Discount();
            TimeLine = new BillingTimeLineDTO();
        }

        public string Uid { get; set; }
        public int OutContractId { get; set; }
        public int ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public decimal? InstallationFee { get; set; }
        public decimal? OtherFee { get; set; }
        public decimal? PackagePrice { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public int? OutletChannelId { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public BillingTimeLineDTO TimeLine { get; set; }
        //public Discount Discount { get; set; }
        public bool HasToCollectMoney { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int? PromotionDetailId { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        public List<CreateUpdateContractTax> OutContractServicePackageTaxes { get; set; }
        public CUOutputChannelPointCommand StartPoint { get; set; }
        public CUOutputChannelPointCommand EndPoint { get; set; }

        public int FlexiblePricingTypeId { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public byte IsDefaultSLAByServiceId { get; set; }
    }
}
