using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CreateDeployNewOutContractCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CreateDeployNewOutContractCommand()
        {
            TransactionServicePackages = new List<CUTransactionServicePackageCommand>();
        }
    }
}
