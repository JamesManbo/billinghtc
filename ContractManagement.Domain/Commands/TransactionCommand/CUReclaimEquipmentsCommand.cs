using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUReclaimEquipmentsCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
    }
}
