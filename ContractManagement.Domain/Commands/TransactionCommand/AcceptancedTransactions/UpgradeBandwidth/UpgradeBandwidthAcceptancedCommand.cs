using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.UpgradeBandwidth
{
    public class UpgradeBandwidthAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse>
    {
        public UpgradeBandwidthAcceptancedCommand()
        {
            TransactionEquipments = new List<CUTransactionEquipmentCommand>();
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }

        public List<CUTransactionEquipmentCommand> TransactionEquipments { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
