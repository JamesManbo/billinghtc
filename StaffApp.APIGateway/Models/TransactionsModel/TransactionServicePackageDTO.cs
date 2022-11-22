using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionServicePackageDTO
    {
        public TransactionServicePackageDTO()
        {
            TransactionEquipments = new List<TransactionEquipmentDTO>();
        }

        public int Id { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public int OutContractId { get; set; }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public BillingTimeLineDTO TimeLine { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public int? OutletChannelId { get; set; }
        public bool HasToCollectMoney { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public MoneyDTO TaxAmount { get; set; }
        public DiscountDTO Discount { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO PackagePrice { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO ExaminedEquipmentAmount { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }
        public MoneyDTO GrandTotalIncludeEquipment { get; set; }
        public MoneyDTO GrandTotalIncludeExaminedEquipment { get; set; }
        public int StatusId { get; set; }
        public string StatusName;
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }

        public MoneyDTO SpInstallationFee { get; set; }
        public MoneyDTO SpPackagePrice { get; set; }
        public MoneyDTO SpSubTotal { get; set; }
        public MoneyDTO SpGrandTotal { get; set; }
        public MoneyDTO EpInstallationFee { get; set; }
        public MoneyDTO EpPackagePrice { get; set; }
        public MoneyDTO EpSubTotal { get; set; }
        public MoneyDTO EpGrandTotal { get; set; }
        public bool? IsOld { get; set; }
        public bool? IsAcceptanced { get; set; }
        public DateTime CreatedBy { get; set; }
        public TransactionChannelPointDTO StartPoint { get; set; }
        public TransactionChannelPointDTO EndPoint { get; set; }
        public List<TransactionEquipmentDTO> TransactionEquipments { get; set; }
    }
}
