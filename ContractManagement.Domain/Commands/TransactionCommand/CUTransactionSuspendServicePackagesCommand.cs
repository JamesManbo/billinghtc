using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTransactionSuspendServicePackagesCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CUTransactionSuspendServicePackagesCommand()
        {
            OutContractIds = new List<int>();
        }
        public decimal? SuspendHandleFee { get; set; }
        public List<int> OutContractIds { get; set; }
    }
}
