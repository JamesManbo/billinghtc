using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions.ChangeLocationServicePackages
{
    public class ChangeLocationServicePackagesAcceptancedCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public ChangeLocationServicePackagesAcceptancedCommand()
        {
            TransactionServicePackagesId = new List<int>();
            AttachmentFiles = new List<CreateUpdateFileCommand>();
        }

        public List<int> TransactionServicePackagesId { get; set; }
        public List<CreateUpdateFileCommand> AttachmentFiles { get; set; }
    }
}
