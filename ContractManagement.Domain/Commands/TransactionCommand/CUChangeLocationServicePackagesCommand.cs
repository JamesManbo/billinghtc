using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUChangeLocationServicePackagesCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public decimal? ChaningLocationFee { get; set; }
    }
}
