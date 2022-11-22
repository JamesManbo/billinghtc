using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTransactionRestoreServicePackagesCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CUTransactionRestoreServicePackagesCommand()
        {
            OutContractIds = new List<int>();
        }
        public string Reason { get; set; }
        public decimal? RestoreHandleFee { get; set; }
        public List<int> OutContractIds { get; set; }
    }
}
