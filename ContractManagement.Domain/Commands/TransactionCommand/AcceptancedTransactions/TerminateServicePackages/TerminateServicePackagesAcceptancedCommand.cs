using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.TerminateServicePackages
{
    public class TerminateServicePackagesAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public TerminateServicePackagesAcceptancedCommand()
        {
        }

        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
