using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Models.OutContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUOutputChannelPointCommand
        : CUDeploymentChannelPointCommand<CUContractEquipmentCommand>, IRequest
    {

        public CUOutputChannelPointCommand()
        {
        }
    }
}
