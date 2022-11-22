using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.MultipleTransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("Transactions")]
    public class Transaction : Entity
    {
        public Transaction()
        {
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
        }

        public void Binding(CUTransactionBaseCommand baseCommand)
        {
            this.CurrencyUnitId = baseCommand.CurrencyUnitId;
            this.CurrencyUnitCode = baseCommand.CurrencyUnitCode;
            this.MarketAreaId = baseCommand.MarketAreaId;
            this.MarketAreaName = baseCommand.MarketAreaName;
            this.ProjectId = baseCommand.ProjectId;
            this.ProjectName = baseCommand.ProjectName;
            this.ContractorId = baseCommand.ContractorId;
            this.Code = baseCommand.Code;
            this.StatusId = baseCommand.StatusId;
            this.TransactionDate = baseCommand.TransactionDate;
            this.OrganizationUnitId = baseCommand.OrganizationUnitId;
            this.HandleUserId = baseCommand.HandleUserId;
            this.Note = baseCommand.Note;
            this.AcceptanceNotes = baseCommand.AcceptanceNotes;
            this.IsTechnicalConfirmation = baseCommand.IsTechnicalConfirmation;
            this.IsSupplierConfirmation = baseCommand.IsSupplierConfirmation;
            this.IsAppendix = baseCommand.IsAppendix;
            this.CreatedBy = baseCommand.CreatedBy;
            this.CreatedDate = baseCommand.CreatedDate;
            this.UpdatedBy = baseCommand.UpdatedBy;
            this.UpdatedDate = baseCommand.UpdatedDate;

            if (baseCommand.OutContractId > 0)
            {
                this.OutContractId = baseCommand.OutContractId;
            }

            if (baseCommand.InContractId > 0)
            {
                this.InContractId = baseCommand.InContractId;
            }

            this.ContractType = baseCommand.ContractType ?? OutContractType.Individual.Id;
            this.ContractCode = baseCommand.ContractCode;
            this.HasEquipment = baseCommand.HasEquipment;

            this.CreatorUserId = baseCommand.CreatorUserId;
            this.TechnicalStaffId = baseCommand.TechnicalStaffId;
            this.ReasonCancelAcceptance = baseCommand.ReasonCancelAcceptance;

            if (Type == TransactionType.TerminateContract.Id)
            {
                this.ReasonType = baseCommand.ReasonType;
                if (string.IsNullOrEmpty(baseCommand.Reason) && this.ReasonType.HasValue)
                {
                    this.Reason = TransactionReason.GetTypeName(ReasonType.Value);
                }
                else
                {
                    this.Reason = baseCommand.Reason;
                }
            }

            if (Type == TransactionType.SuspendServicePackage.Id)
            {
                this.ReasonType = baseCommand.ReasonType;
                if (string.IsNullOrEmpty(baseCommand.Reason) && this.ReasonType.HasValue)
                {
                    this.Reason = TransactionReason.GetTypeName(ReasonType.Value);
                }
                else
                {
                    this.Reason = baseCommand.Reason;
                }
            }
        }
        public void Binding(BaseMultipleTransactionCommand baseCommand)
        {
            this.TransactionDate = DateTime.UtcNow.AddHours(7);
            this.AutoConfirmation = baseCommand.AutoConfirmation;
            this.ProjectId = baseCommand.ProjectId;
            this.ProjectName = baseCommand.ProjectName;
            this.IsTechnicalConfirmation = baseCommand.IsTechnicalConfirmation;
            if (this.IsTechnicalConfirmation == true)
            {
                this.StatusId = TransactionStatus.WaitAcceptanced.Id;
            }
            else if (this.AutoConfirmation == true)
            {
                this.StatusId = TransactionStatus.AcceptanceApproved.Id;
                this.EffectiveDate = DateTime.UtcNow.AddHours(7);
            }
            else
            {
                this.StatusId = TransactionStatus.Acceptanced.Id;
            }

            this.IsSupplierConfirmation = false;
            this.Note = baseCommand.Note;
            this.IsAppendix = false;
            this.CreatedBy = baseCommand.CreatedBy;
            this.CreatedDate = baseCommand.CreatedDate;

            this.CreatorUserId = baseCommand.CreatorUserId;
            this.IsMultipleTransaction = true;
        }

        public Transaction(MultipleChangeEquipmentCommand command)
        {
            this.Type = TransactionType.ChangeEquipment.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(MultipleReclaimEquipmentCommand command)
        {
            this.Type = TransactionType.ReclaimEquipment.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(MultipleTerminateServiceCommand command)
        {
            this.Type = TransactionType.TerminateServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(MultipleRestoreChannelCommand command)
        {
            this.Type = TransactionType.RestoreServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(MultipleUpgradePackageCommand command)
        {
            this.Type = TransactionType.ChangeServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(MultipleSuspendChannelCommand command)
        {
            this.Type = TransactionType.SuspendServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(CUAddNewServicePackageTransaction command)
        {
            this.Type = TransactionType.AddNewServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(CUChangeServicePackageTransaction command)
        {
            this.Type = TransactionType.ChangeServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.ChangingPackageFee = command.ChangingPackageFee;
        }
        public Transaction(CreateDeployNewOutContractCommand command)
        {
            this.Type = TransactionType.DeployNewOutContract.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(CUUpgradeBandwidthsCommand command)
        {
            this.Type = TransactionType.UpgradeBandwidth.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.UpgradeFee = command.UpgradeFee;
        }

        public Transaction(CUTransactionTerminateServicePackagesCommand command)
        {
            this.Type = TransactionType.TerminateServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.ReasonType = command.ReasonType;
            if (string.IsNullOrEmpty(command.Reason) && this.ReasonType.HasValue)
            {
                this.Reason = TransactionReason.GetTypeName(ReasonType.Value);
            }
            else
            {
                this.Reason = command.Reason;
            }
        }

        public Transaction(CUTransactionRestoreServicePackagesCommand command)
        {
            this.Type = TransactionType.RestoreServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.Reason = command.Reason;
            this.RestoreHandleFee = command.RestoreHandleFee;
        }

        public Transaction(CUTransactionSuspendServicePackagesCommand command)
        {
            this.Type = TransactionType.SuspendServicePackage.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.SuspendHandleFee = command.SuspendHandleFee;
        }

        public Transaction(CUChangeLocationServicePackagesCommand command)
        {
            this.Type = TransactionType.ChangeLocation.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.ChaningLocationFee = command.ChaningLocationFee;
            this.HasEquipment = command.HasEquipment;
        }

        public Transaction(CUReclaimEquipmentsCommand command)
        {
            this.Type = TransactionType.ReclaimEquipment.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }

        public Transaction(CUChangeEquipmentsCommand command)
        {
            this.Type = TransactionType.ChangeEquipment.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
            this.ChangeEquipmentFee = command.ChangeEquipmentFee;
        }

        public Transaction(CUTerminateContractCommand command)
        {
            this.Type = TransactionType.TerminateContract.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);
        }
        public Transaction(CURenewContractCommand command)
        {
            this.Type = TransactionType.RenewContract.Id;
            TransactionServicePackages = new HashSet<TransactionServicePackage>();
            AttachmentFiles = new HashSet<AttachmentFile>();
            this.Binding(command);

            this.RenewFee = command.RenewFee;
            this.ContractExpiredDate = command.ContractExpiredDate;
            this.ContractRenewMonths = command.ContractRenewMonths;
            this.ContractNewExpirationDate = command.ContractNewExpirationDate;
        }

        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CreatorUserId { get; set; }
        public int? ContractorId { get; set; }
        public int ContractType { get; set; }
        public string ContractCode { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int StatusId { get; set; }
        public string HandleUserId { get; set; }
        public string AcceptanceStaff { get; set; }
        public string OrganizationUnitId { get; set; }
        public int? ReasonType { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public string AcceptanceNotes { get; set; }
        public decimal? SuspendHandleFee { get; set; }
        public decimal? RestoreHandleFee { get; set; }
        public decimal? ChaningLocationFee { get; set; }
        public decimal? ChangeEquipmentFee { get; set; }
        public decimal? UpgradeFee { get; set; }
        public decimal? ChangingPackageFee { get; set; }
        public decimal? RenewFee { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public bool? IsAppendix { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public bool? HasEquipment { get; set; }
        public string TechnicalStaff { get; set; }
        public string TechnicalStaffId { get; set; }
        public string ReasonCancelAcceptance { get; set; }

        public DateTime ContractExpiredDate { get; set; }
        public int ContractRenewMonths { get; set; }
        public DateTime ContractNewExpirationDate { get; set; }
        public bool IsMultipleTransaction { get; set; }
        public bool AutoConfirmation { get; set; }

        public virtual HashSet<TransactionServicePackage> TransactionServicePackages { get; set; }
        public virtual HashSet<AttachmentFile> AttachmentFiles { get; set; }

        public void AddTransServicePackage(CUTransactionServicePackageCommand command)
        {
            var newTransServicePackage = new TransactionServicePackage(command)
            {
                ProjectId = this.ProjectId
            };
            this.TransactionServicePackages.Add(newTransServicePackage);
        }

        public void UpdateServicePackage(CUTransactionServicePackageCommand updateChannelCmd, bool forceBind = false)
        {
            var channelEntity = this.TransactionServicePackages.First(s => s.Id == updateChannelCmd.Id);
            channelEntity.ProjectId = this.ProjectId;
            channelEntity.UpdatedBy = this.UpdatedBy;
            channelEntity.UpdatedDate = DateTime.Now;

            channelEntity.Update(updateChannelCmd, forceBind);
            channelEntity.CalculateTotal();
        }
    }
}
