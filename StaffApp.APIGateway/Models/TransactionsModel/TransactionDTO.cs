using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionDTO
    {
        public TransactionDTO()
        {
            TransactionServicePackages = new List<TransactionServicePackageDTO>();
            AttachmentFiles = new List<TransactionAttachmentFileDTO>();
            TransactionEquipments = new List<TransactionEquipmentDTO>();
            OutContractIds = new List<int>();
            OutContractCodes = new List<string>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string TypeName;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int StatusId { get; set; }
        public string StatusName;
        public string CreatedBy { get; set; }
        public string HandleUserId { get; set; }
        public string AcceptanceStaff { get; set; }
        public string OrganizationUnitId { get; set; }
        public int? ReasonType { get; set; }
        public string Reason { get; set; }
        public string ReasonCancelAcceptance { get; set; }
        public string Note { get; set; }
        public MoneyDTO SuspendHandleFee { get; set; }
        public MoneyDTO RestoreHandleFee { get; set; }
        public MoneyDTO ChaningLocationFee { get; set; }
        public MoneyDTO ChangeEquipmentFee { get; set; }
        public MoneyDTO UpgradeFee { get; set; }
        public ContractorDTO Contractor { get; set; }

        public List<TransactionServicePackageDTO> TransactionServicePackages { get; set; }
        public List<TransactionAttachmentFileDTO> AttachmentFiles { get; set; }
        public List<TransactionEquipmentDTO> TransactionEquipments { get; set; }
        public List<int> OutContractIds { get; set; }
        public List<string> OutContractCodes { get; set; }
        public string ContractCode { get; set; }
        public string AcceptanceNotes { get; set; }
    }
}
