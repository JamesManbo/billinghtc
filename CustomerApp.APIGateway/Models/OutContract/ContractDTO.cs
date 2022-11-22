using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public string ContractCode { get; set; }
        public string AgentCode { get; set; }
        public int ContractTypeId { get; set; }
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string OrganizationUnitId { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int ContractorId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int PaymentMethodId { get; set; }
        public string Description { get; set; }
        public string SignedUserId { get; set; }
        public int SalesmanId { get; set; }
        public string ContractNote { get; set; }
        public float TotalTaxPercent { get; set; }
        public int ContractStatus { get; set; }
        public MoneyDTO TotalTaxAmount { get; set; }
        public MoneyDTO InstallationFee { get; set; }
        public MoneyDTO OtherFee { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotalBeforeTax { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public DiscountDTO Discount { get; set; }
        public PaymentMethodDTO Payment { get; set; }
        public ContractTimeLineDTO TimeLine { get; set; }
        public ContractorDTO Contractor { get; set; }
        public ContractorDTO ContractorHTC { get; set; }
        public string FiberNodeInfo { get; set; }
        public string AgentContractCode { get; set; }
        public string CashierUserId { get; set; }
        public MoneyDTO EquipmentAmount { get; set; }
        public MoneyDTO ServicePackageAmount { get; set; }
        public List<OutContractServicePackageDTO> ServicePackages { get; set; }
        public List<OutContractEquipmentDTO> Equipments { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public List<ContractOfTaxDTO> ContractOfTaxes { get; set; }
        public string ContractStatusName { get; set; }
        public string SignedUserName { get; set; }
        public string OrganizationUnitName { get; set; }
        public string CashierUserName { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public ContractContentDTO ContractContent { get; set; }
    }
}
