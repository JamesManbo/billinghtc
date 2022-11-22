using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;

namespace ContractManagement.Domain.Models
{
    public abstract class ContractBaseDTO : BaseDTO
    {
        protected ContractBaseDTO()
        {
            AttachmentFiles = new List<AttachmentFileDTO>();
            ContactInfos = new List<ContactInfoDTO>();
            TaxCategories = new List<TaxCategoryDTO>();
            ContractTotalByCurrencies = new List<ContractTotalByCurrencyDTO>();
        }
        public string Direction;
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public bool HaveEquipment { get; set; }

        public int ContractStatus { get; set; }
        public string ContractCode { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int? ContractTypeId { get; set; }
        public string ContractTypeName { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; set; }
        public string OrganizationUnitCode { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName 
            => AggregatesModel.BaseContract.ContractStatus.From(ContractStatusId).Name ?? "";
        public string ContractorIdentityGuid { get; set; }
        public int ContractorId { get; set; }
        public int ContractorHTCId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Description { get; set; }
        public string SignedUserId { get; set; }
        public string SignedUserName { get; set; }
        public int? SalesmanId { get; set; }
        public string ContractNote { get; set; }
        public float TotalTaxPercent { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public bool AutoRenew{ get; set; }
        public PaymentMethod Payment { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public ContractorDTO Contractor { get; set; }
        public ContractorDTO ContractorHTC { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public List<TaxCategoryDTO> TaxCategories { get; set; }
        public List<ContactInfoDTO> ContactInfos { get; set; }
        public bool IsIncidentControl { get; set; }
        public bool IsControlUsageCapacity { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public ContractContentDTO ContractContent { get; set; }
        public List<ContractTotalByCurrencyDTO> ContractTotalByCurrencies { get; set; }
        public string AccountingCustomerCode { get; set; }
    }
}
