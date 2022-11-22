using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.ChangeEquipments
{
    public class ChangeEquipmentsAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public ChangeEquipmentsAcceptancedCommand()
        {
            TransactionEquipmentsId = new List<int>();
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }

        public List<int> TransactionEquipmentsId { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
