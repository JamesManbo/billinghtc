using System;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.BaseContractCommand;
using MediatR;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUContractEquipmentCommand : CUDeploymentEquipmentCommand, IRequest
    {
        public CUContractEquipmentCommand(TransactionEquipment transEquipment) 
            : base(transEquipment)
        {
            this.TransactionEquipmentId = transEquipment.Id;
        }

        public CUContractEquipmentCommand()
        {
        }

        public int? TransactionEquipmentId { get; set; }
    }
}
