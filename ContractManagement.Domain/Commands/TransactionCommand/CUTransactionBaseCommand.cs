using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public abstract class CUTransactionBaseCommand
    {
        public CUTransactionBaseCommand()
        {
            TransactionServicePackages = new List<CUTransactionServicePackageCommand>();
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }
        public ContractorDTO Contractor { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int Id { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ContractorId { get; set; }
        public string CreatorUserId { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int StatusId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string OrganizationUnitId { get; set; }
        public string HandleUserId { get; set; }
        public string Note { get; set; }
        public string AcceptanceNotes { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public bool? IsAppendix { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AcceptanceStaff { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public int? ContractType { get; set; }
        public string ContractCode { get; set; }
        public bool? HasEquipment { get; set; }
        public string TechnicalStaffId { get; set; }
        public int? ReasonType { get; set; }
        public string Reason { get; set; }
        public string ReasonCancelAcceptance { get; set; }

        public List<CUTransactionServicePackageCommand> TransactionServicePackages { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
