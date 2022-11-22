using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;

namespace ContractManagement.Domain.Models
{
    public class OutContractGridDTO : BaseDTO
    {
        public OutContractGridDTO()
        {
            ServicePackages = new List<ContractPackageGridDTO>();
            Direction = "out";
        }
        public readonly string Direction;
        public int? ContractTypeId { get; set; }
        public ContractorDTO Contractor { get; set; }
        public string ContractCode { get; set; }
        public string AgentCode { get; set; }
        public string AgentContractCode { get; set; }
        public string MarketAreaName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string SignedUserName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public decimal GrandTotal { get; set; }
        public bool HasUploadedFiles { get; set; }
        public List<ContractPackageGridDTO> ServicePackages { get; set; }

    }
    public class ContractPackageGridDTO : BaseDTO
    {
        public ContractPackageGridDTO()
        {
        }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ContractCode { get; set; }
        public int OutContractId { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int? ServiceId { get; set; }
        public int? ServicePackageId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public int PackagePriceType { get; set; }
        public decimal OtherFee { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public float DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public bool IsRadiusAccountCreated { get; set; }
        public int StatusId { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public OutputChannelPointDTO StartPoint { get; set; }
        public OutputChannelPointDTO EndPoint { get; set; }
        public BillingTimeLine TimeLine { get; set; }
        public int PromotionDetailId { get; set; }
        public int FlexiblePricingTypeId { get; set; }
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
                    _value += "chưa nghiệm thu";
                }

                if (!string.IsNullOrEmpty(this.PackageName))
                {
                    _value += $", gói cước { this.PackageName}";
                }

                _value += $", dịch vụ { this.ServiceName}";
                _value += ". Địa chỉ:";
                if (HasStartAndEndPoint)
                {
                    _value += $"{ StartPoint.InstallationAddress.FullAddress}- ";
                }
                _value += $"{ EndPoint.InstallationAddress.FullAddress}";
                return _value;
            }
        }
    }
}
