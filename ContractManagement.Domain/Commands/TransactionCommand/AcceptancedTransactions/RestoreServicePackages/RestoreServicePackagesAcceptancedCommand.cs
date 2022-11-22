using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.RestoreServicePackages
{
    public class RestoreServicePackagesAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public RestoreServicePackagesAcceptancedCommand()
        {
        }

        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
