using System;
using System.Collections.Generic;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models.OutContracts;
using MediatR;

namespace ContractManagement.Domain.Models
{
    public class OutContractServicePackageDTO : BaseDTO, IRequest
    {
        public string Uid { get; set; }
        public int? ContractorId { get; set; }
        public int? ContractType { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ContractCode { get; set; }
        public ServiceChannelType Type { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public int? ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public bool IsRadiusAccountCreated { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public bool IsHasPrice { get; set; }
        public string PricingTypeName { get; set; }
        public decimal PackagePrice { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public int DaysSuspended { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsInFirstBilling { get; set; }
        public int StatusId { get; set; }
        public string StatusName => OutContractServicePackageStatus.From(StatusId).Name;

        public string Note { get; set; }
        public string OtherNote { get; set; }
        //promotion
        public int? PromotionDetailId { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public int? OldId { get; set; }
        public bool? IsOldOCSPCheaper { get; set; }
        public int? RadiusServerId { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? MinSubTotal { get; set; }
        public decimal? MaxSubTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public int ChannelGroupId { get; set; }
        public string ChannelGroupName { get; set; }
        public int PaymentTargetId { get; set; }
        public int? StartPointChannelId { get; set; }
        public int EndPointChannelId { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public OutputChannelPointDTO StartPoint { get; set; }
        public OutputChannelPointDTO EndPoint { get; set; }
        public List<OutContractServicePackageTaxDTO> OutContractServicePackageTaxes { get; set; }
        public List<PromotionForContractDTO> PromotionForContracts { get; set; }
        public List<ChannelPriceBusTableDTO> PriceBusTables { get; set; }

        public ContractorDTO PaymentTarget { get; set; }
        public byte IsDefaultSLAByServiceId { get; set; }

        public string InstallationAddressSpliter { get; set; }
        public int FlexiblePricingTypeId { get; set; }

        public OutContractServicePackageDTO()
        {
            OutContractServicePackageTaxes = new List<OutContractServicePackageTaxDTO>();
            PromotionForContracts = new List<PromotionForContractDTO>();
            PriceBusTables = new List<ChannelPriceBusTableDTO>();
            ContractCode = "";
        }

        public string Text
        {
            get
            {
                var _value = "Kênh ";

                if (!string.IsNullOrEmpty(this.CId))
                {
                    _value += $"{ this.CId}";
                }
                else
                {
                    _value += "chưa triển khai";
                }

                if (!string.IsNullOrEmpty(this.PackageName))
                {
                    _value += $", gói cước { this.PackageName}";
                }

                _value += $", dịch vụ { this.ServiceName}";
                _value += ". Địa chỉ: ";
                if (HasStartAndEndPoint)
                {
                    _value += $"{ StartPoint?.InstallationAddress?.FullAddress ?? string.Empty} - ";
                }
                _value += $"{ EndPoint?.InstallationAddress?.FullAddress ?? string.Empty}";
                return _value;
            }
        }

        public object Value => Id > 0 ? Id : (object)Guid.NewGuid().ToString();
    }
}