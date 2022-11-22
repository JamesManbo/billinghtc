using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.TaxCategoryModels;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public int ContractStatus { get; set; }
        public string ContractCode { get; set; }
        public string AgentCode { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentContractCode { get; set; }
        public int? ContractTypeId { get; set; }
        public int? MarketAreaId { get; protected set; }
        public string MarketAreaName { get; protected set; }
        public int? ProjectId { get; protected set; }
        public string ProjectName { get; protected set; }
        public string OrganizationUnitId { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public int? ContractorHTCId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Description { get; set; }
        public string SignedUserId { get; protected set; }
        public int? SalesmanId { get; protected set; }
        public string ContractNote { get; set; }
        public string OrganizationUnitName { get; set; }
        public string ContractStatusName { get; set; }
        public string FiberNoteInfo { get; set; }
        public int NumberBillingLimitDays { get; set; }

        public float TotalTaxPercent { get; protected set; }
        public MoneyDTO TotalTaxAmount { get; protected set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO SubTotalBeforeTax { get; set; }
        public MoneyDTO GrandTotalBeforeTax { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public Discount Discount { get; set; }
        public PaymentMethod Payment { get; set; }
        public ContractTimeLineDTO TimeLine { get; set; }
        public ContractorDTO Contractor { get; set; }
        public ContractorDTO ContractorHTC { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public List<TaxCategoryDTO> TaxCategories { get; set; }
        public List<OutContractServicePackageDTO> ServicePackages { get; set; }
        public ContractContentDTO ContractContent { get; set; }
    }
}
