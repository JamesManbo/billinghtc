using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.ReclaimEquipments
{
    public class ReclaimEquipmentsAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public ReclaimEquipmentsAcceptancedCommand()
        {
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
