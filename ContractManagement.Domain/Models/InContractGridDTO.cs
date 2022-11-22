using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Models.SharingRevenueModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class InContractGridDTO : BaseDTO
    {
        public InContractGridDTO()
        {
            ServicePackages = new List<ContractPackageGridDTO>();
            ContractSharingRevenues = new List<ContractSharingRevenueGridDTO>();
            //InContractServices = new List<InContractServiceDTO>();
        }

        public ContractorDTO Contractor { get; set; }
        public int? ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public string ContractCode { get; set; }
        public string AgentCode { get; set; }
        //public string AgentContractCode { get; set; }
        public string MarketAreaName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SignedUserName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public decimal GrandTotal { get; set; }
        public List<ContractPackageGridDTO> ServicePackages { get; set; }
        public List<ContractSharingRevenueGridDTO> ContractSharingRevenues { get; set; }
        public bool HasUploadedFiles { get; set; }
        //public List<InContractServiceDTO> InContractServices { get; set; }
    }

    public class ContractSharingRevenueGridDTO : BaseDTO
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
        public decimal SharedMaintenanceFee { get; set; }
        public Address Address { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }
}
