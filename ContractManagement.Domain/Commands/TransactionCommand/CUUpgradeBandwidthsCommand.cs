using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUUpgradeBandwidthsCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CUUpgradeBandwidthsCommand()
        {
            OutContractIds = new List<int>();
        }

        public decimal? UpgradeFee { get; set; }
        public List<int> OutContractIds { get; set; }
        public List<CUTransactionServicePackageCommand> TransactionServicePackages { get; set; }
    }
}
