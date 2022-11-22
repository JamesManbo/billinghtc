using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUTransactionTerminateServicePackagesCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CUTransactionTerminateServicePackagesCommand()
        {
        }
        public int ReasonType { get; set; }
        public string Reason { get; set; }
    }
}
