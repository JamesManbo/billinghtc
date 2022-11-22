using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.ContractContentCommand;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Models;

namespace ContractManagement.Domain.Commands.BaseContractCommand
{
    public abstract class CreateUpdateContractBaseCommand
    {
        protected CreateUpdateContractBaseCommand()
        {
            AttachmentFiles = new List<CreateUpdateFileCommand>();
            ServiceLevelAgreements = new List<CUServiceLevelAgreementCommand>();
            ServicePackages = new List<CUOutContractChannelCommand>();
            ContactInfos = new List<CUContactInfoCommand>();
            TaxCategories = new List<CUOutContractServicePackageTaxCommand>();
            ContractSharingRevenues = new List<CUContractSharingRevenueLineCommand>();
            DeletedContractSharingRevenues = new List<int>();
        }

        public int Id { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int ContractTypeId { get; set; }
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; set; }
        public string OrganizationUnitCode { get; set; }
        public string SignedUserName { get; set; }
        public string ContractCode { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public string Description { get; set; }
        public string SignedUserId { get; set; }
        public int? SalesmanId { get; set; }

        public int? InterestOnDefferedPayment { get; set; }
        public int? ContractViolation { get; set; }
        public int? ContractViolationType { get; set; }

        public string ContractNote { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public PaymentMethod Payment { get; set; }
        public CUContractorCommand Contractor { get; set; }
        public CUContractorHTCCommand ContractorHTC { get; set; }
        public string InvoicingAddress { get; set; }
        public bool IsIncidentControl { get; set; }
        public bool IsControlUsageCapacity { get; set; }
        public int NumberBillingLimitDays { get; set; }

        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public CUContractContentCommand ContractContentCommand { get; set; }
        public List<CUOutContractChannelCommand> ServicePackages { get; set; }
        public List<CUServiceLevelAgreementCommand> ServiceLevelAgreements { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
        public List<CUContactInfoCommand> ContactInfos { get; set; }
        public List<CUOutContractServicePackageTaxCommand> TaxCategories { get; set; }
        public List<CUContractSharingRevenueLineCommand> ContractSharingRevenues { get; set; }
        public List<int> DeletedContractSharingRevenues { get; set; }
        public int[] DeletedServicePackages { get; set; }
        public int[] DeletedAttachments { get; set; }
        public int[] DeletedSLAs { get; set; }
        public bool AutoRenew { get; set; } = false;
        public string AccountingCustomerCode { get; set; }
    }
}
