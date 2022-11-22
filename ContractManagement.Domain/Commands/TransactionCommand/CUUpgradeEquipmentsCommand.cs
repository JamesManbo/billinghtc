using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionCommand
{
    public class CUUpgradeEquipmentsCommand : CUTransactionBaseCommand, IRequest<ActionResponse<TransactionDTO>>
    {
        public CUUpgradeEquipmentsCommand()
        {
            OutContractIds = new List<int>();
        }
        public decimal? ChangeEquipmentFee { get; set; }
        public List<int> OutContractIds { get; set; }
        public List<CUTransactionEquipmentCommand> TransactionEquipments { get; set; }
    }
}
