using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;

namespace ContractManagement.Domain.Models.SharingRevenueModels
{
    public class OutContractSharingRevenueDTO
    {
        public OutContractSharingRevenueDTO()
        {
            this.ContractStatusId = AggregatesModel.BaseContract.ContractStatus.Draft.Id;
            this.ServicePackages = new List<OutContractServicePackageDTO>();
            this.SharingRevenueLines = new List<ContractSharingRevenueLineDTO>();
        }

        public int OutContractId { get; set; }
        public int ContractStatus { get; set; }
        public string ContractCode { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int? ContractTypeId { get; set; }
        public int? MarketAreaId { get; protected set; }
        public string MarketAreaName { get; protected set; }
        public int? ProjectId { get; protected set; }
        public string ProjectName { get; protected set; }
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; protected set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName => AggregatesModel.BaseContract.ContractStatus.From(ContractStatusId).Name;
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string ContractorFullName { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? PaymentMethodId { get; set; }
        public string ContractNote { get; set; }
        public float TotalTaxPercent { get; protected set; }
        public decimal TotalTaxAmount { get; protected set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public int SharingRevenueId { get; set; }
        public int SharingType { get; set; }
        public decimal TotalSharingAmount { get; set; }
        public decimal OutSharedFixedAmount { get; set; }
        public decimal InSharedFixedAmount { get; set; }
        public List<OutContractServicePackageDTO> ServicePackages { get; set; }
        public List<ContractSharingRevenueLineDTO> SharingRevenueLines { get; set; }
    }
}
