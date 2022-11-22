using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTransactionCommand : IRequest<ActionResponse<TransactionDTO>>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int StatusId { get; set; }
        public string HandleUserId { get; set; }
        public string AcceptanceStaff { get; set; }
        public string OrganizationUnitId { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public int? OutContractId { get; set; }

        public List<CUTransactionEquipmentCommand> TransactionEquipments { get; set; }
        public List<CUTransactionServicePackageCommand> TransactionServicePackages { get; set; }
        public List<CUTransactionPromotionForContractCommand> TransactionPromotionForContracts { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<int> ContractIds { get; set; }
        public bool? HasEquipment { get; set; }
    }
}
