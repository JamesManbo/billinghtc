using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.SuspendServicePackages
{
    public class SuspendServicePackagesAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public SuspendServicePackagesAcceptancedCommand()
        {
        }

        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
