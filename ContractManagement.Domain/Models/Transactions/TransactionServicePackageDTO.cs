using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.Transactions;
using System;
using System.Collections.Generic;

namespace ContractManagement.Domain.Models
{
    public class TransactionServicePackageDTO : BaseDTO
    {
        public TransactionServicePackageDTO()
        {
            TransactionEquipments = new List<TransactionEquipmentDTO>();
            TransactionPromotionForContracts = new List<TransactionPromotionForContactDTO>();
            TimeLine = new BillingTimeLine();
            TransactionChannelTaxes = new List<TransactionChannelTaxDTO>();
            PriceBusTables = new List<TransactionPriceBusTableDTO>();
        }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutContractServicePackageId { get; set; }
        public int TransactionId { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
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
        public string StatusName => OutContractServicePackageStatus.From(StatusId).Name;
        public bool? IsOld { get; set; }
        public bool? IsAcceptanced { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }

        public int PaymentTargetId { get; set; }
        public int? StartPointChannelId { get; set; }
        public int EndPointChannelId { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public int FlexiblePricingTypeId { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public decimal? MinSubTotal { get; set; }
        public decimal? MaxSubTotal { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public TransactionChannelPointDTO StartPoint { get; set; }
        public TransactionChannelPointDTO EndPoint { get; set; }
        public ContractorDTO PaymentTarget { get; set; }
        public List<TransactionEquipmentDTO> TransactionEquipments { get; set; }
        public List<TransactionChannelTaxDTO> TransactionChannelTaxes { get; set; }
        public List<TransactionPriceBusTableDTO> PriceBusTables { get; set; }

        public List<TransactionPromotionForContactDTO> TransactionPromotionForContracts { get; set; }
    }
}
