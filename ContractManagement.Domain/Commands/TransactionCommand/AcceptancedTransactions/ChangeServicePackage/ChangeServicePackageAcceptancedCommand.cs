using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.ChangeServicePackage
{
   public class ChangeServicePackageAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public ChangeServicePackageAcceptancedCommand()
        {
            TransactionEquipments = new List<CUTransactionEquipmentCommand>();
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }

        public List<CUTransactionEquipmentCommand> TransactionEquipments { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
