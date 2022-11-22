using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class OutContractServicePackageDTO
    {
        public int Type { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutContractId { get; set; }
        public int ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public string Note { get; set; }
        public int OutletChannelId { get; set; }
        public bool HasToCollectMoney { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public int FlexiblePricingTypeId { get; set; }
        public decimal? MinSubTotal { get; set; }
        public decimal? MaxSubTotal { get; set; }
        public MoneyDTO OrgPackagePrice { get; set; }
        public MoneyDTO PackagePrice { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public DiscountDTO Discount { get; set; }
        public BillingTimeLineDTO TimeLine { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        
        public MoneyDTO SpInstallationFee { get; set; }
        public MoneyDTO SpPackagePrice { get; set; }
        public MoneyDTO SpSubTotal { get; set; }
        public MoneyDTO SpGrandTotalBeforeTax { get; set; }
        public MoneyDTO SpGrandTotal { get; set; }
        public MoneyDTO EpInstallationFee { get; set; }
        public MoneyDTO EpPackagePrice { get; set; }
        public MoneyDTO EpSubTotal { get; set; }
        public MoneyDTO EpGrandTotalBeforeTax { get; set; }
        public MoneyDTO EpGrandTotal { get; set; }
        public MoneyDTO GrandTotalIncludeEquipment { get; set; }
        public MoneyDTO GrandTotalIncludeExaminedEquipment { get; set; }
        public OutputChannelPointDTO StartPoint { get; set; }
        public OutputChannelPointDTO EndPoint { get; set; }
    }
}
