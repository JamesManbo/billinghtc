using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class TransactionServicePackageDTO : BaseDTO
    {
        public TransactionServicePackageDTO()
        {
            TimeLine = new BillingTimeLine();
        }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public int OutContractId { get; set; }
        public int ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal ExaminedEquipmentAmount { get; set; }
        public decimal EquipmentAmount { get; set; }
        public int StatusId { get; set; }

        //public bool? IsOld { get; set; }
        //public bool? IsAcceptanced { get; set; }
        //public DateTime CreatedBy { get; set; }
        //public string RadiusAccount { get; set; }
        //public string RadiusPassword { get; set; }
        //public bool? IsTechnicalConfirmation { get; set; }

        //public int PaymentTargetId { get; set; }
        //public int? StartPointChannelId { get; set; }
        //public int EndPointChannelId { get; set; }
        //public float LineQuantity { get; set; } // số lượng tuyến cáp
        //public float? CableKilometers { get; set; } // số kilomet cáp
        //public int FlexiblePricingTypeId { get; set; }
        //public bool HasDistinguishBandwidth { get; set; }
        //public decimal? MinSubTotal { get; set; }
        //public decimal? MaxSubTotal { get; set; }
    }
}
