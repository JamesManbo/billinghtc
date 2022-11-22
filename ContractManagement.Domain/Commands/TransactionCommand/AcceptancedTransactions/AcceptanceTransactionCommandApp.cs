using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions
{
    public class AcceptanceTransactionCommandApp: IRequest<ActionResponse<TransactionDTO>>
    {
        public AcceptanceTransactionCommandApp()
        {
            AttachmentFiles = new List<CreateUpdateFileCommand>();
            Equipments = new List<TransactionEquipmentDTO>();
        }

        public int TransactionId { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
        public List<TransactionEquipmentDTO> Equipments { get; set; }
        public string Note { get; set; }
        public string AcceptanceStaff { get; set; }
        public string AcceptanceStaffUid { get; set; }
    }
}
