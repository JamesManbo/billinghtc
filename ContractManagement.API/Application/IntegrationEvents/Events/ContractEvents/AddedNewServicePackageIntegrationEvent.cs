using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.IntegrationEvents.Events
{
    public class AddedNewServicePackageIntegrationEvent : IntegrationEvent
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutContractId { get; set; }
        public int ContractStatus { get; set; }
        public string ContractCode { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int? ContractTypeId { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public string MarketAreaCode { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Description { get; set; }
        public string SignedUserId { get; set; }
        public string SignedUserName { get; set; }
        public int? SalesmanId { get; set; }
        public string ContractNote { get; set; }
        public string AgentContractCode { get; set; }// Mã hợp đồng đại lý
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public decimal EquipmentAmount { get; set; }
        public decimal ServicePackageAmount { get; set; }
        public float TotalTaxPercent { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
        public PaymentMethod Payment { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public ContractorDTO Contractor { get; set; }
        public List<TaxCategoryDTO> TaxCategories { get; set; }
        public List<ContractSharingRevenueLineDTO> ContractSharingRevenues { get; set; }
        public OutContractServicePackageDTO NewServicePackage { get; set; }
        public List<TransactionPromotionForContactDTO> PromotionForContactDTOs { get; set; }
        public int NumberBillingLimitDays { get; set; }
    }
}
