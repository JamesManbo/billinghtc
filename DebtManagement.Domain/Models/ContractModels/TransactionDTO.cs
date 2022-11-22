using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class TransactionDTO : BaseDTO
    {
        public TransactionDTO()
        {
            TransactionServicePackages = new List<TransactionServicePackageDTO>();
        }        
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int ContractType { get; set; }
        public string ContractCode { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int StatusId { get; set; }
        public string CreatedBy { get; set; }
        public string HandleUserId { get; set; }
        public string AcceptanceStaff { get; set; }
        public string OrganizationUnitId { get; set; }
        public int? ReasonType { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string AcceptanceNotes { get; set; }
        public string PromotionDescription { get; set; }
        public decimal? SuspendHandleFee { get; set; }
        public decimal? RestoreHandleFee { get; set; }
        public decimal? ChaningLocationFee { get; set; }
        public decimal? ChangeEquipmentFee { get; set; }
        public decimal? UpgradeFee { get; set; }
        public decimal? ChangingPackageFee { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsAppendix { get; set; }
        public int? OutContractId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public ContractorDTO Contractor { get; set; }
        public List<TransactionServicePackageDTO> TransactionServicePackages { get; set; }
        public List<AttachmentFileDTO> AttachmentFiles { get; set; }
        public bool? HasEquipment { get; set; }
        public int ContractorId { get; set; }
        public string ContractorFullName { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        //public int Id { get; set; }
        //public string Code { get; set; }
        //public int Type { get; set; }
        //public string TypeName { get; set; }
        //public DateTime TransactionDate { get; set; }
        //public DateTime? EffectiveDate { get; set; }
        //public int StatusId { get; set; }
        //public string StatusName { get; set; }
        //public string CreatedBy { get; set; }
        //public string HandleUserId { get; set; }
        //public string AcceptanceStaff { get; set; }
        //public string OrganizationUnitId { get; set; }
        //public int? ReasonType { get; set; }
        //public string Reason { get; set; }
        //public string Note { get; set; }
        //public string PromotionDescription { get; set; }
        //public decimal? SuspendHandleFee { get; set; }
        //public decimal? RestoreHandleFee { get; set; }
        //public decimal? ChaningLocationFee { get; set; }
        //public decimal? ChangeEquipmentFee { get; set; }
        //public decimal? UpgradeFee { get; set; }
    }
}
