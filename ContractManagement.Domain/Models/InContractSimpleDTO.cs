using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class InContractSimpleDTO : BaseDTO
    {
        public InContractSimpleDTO()
        {
        }

        public ContractorSimpleDTO Contractor { get; set; }
        public int? ContractTypeId { get; protected set; }
        public string ContractCode { get; protected set; }
        public string MarketAreaName { get; protected set; }
        public string ProjectName { get; protected set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public decimal GrandTotal { get; set; }
        public bool HasUploadedFiles { get; set; }

    }

    public class ContractChannelSimpleDTO : BaseDTO
    {
        public int? InContractId { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int Units { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public decimal RentalCharge { get; set; }
        public decimal RentalChargeBeforeTax { get; set; }
        public Address Address { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }

    public class ContractSharingRevenueSimpleDTO : BaseDTO
    {
        public int InContractId { get; set; }
        public int? OutContractId { get; set; }
        public int SharingType { get; set; }
        public int? ServiceId { get; set; }
        public int? InServiceChannelId { get; set; }
        public int? OutServiceChannelId { get; set; }
        public decimal SharedChannelRentalFee { get; set; }
        public float CommissionPercent { get; set; }
        public float InstallFeeCommissionPercent { get; set; }
        public float SharedInstallFeePercent { get; set; }
        public float SharedPackageChargePercent { get; set; }
        public Address Address { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }
}
