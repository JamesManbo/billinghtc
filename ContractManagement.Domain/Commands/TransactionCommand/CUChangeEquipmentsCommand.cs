using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUChangeEquipmentsCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public decimal? ChangeEquipmentFee { get; set; }
    }
}
