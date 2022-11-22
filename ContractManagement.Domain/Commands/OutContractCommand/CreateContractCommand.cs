using System.Collections.Generic;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CreateContractCommand : CreateUpdateContractBaseCommand, IRequest<ActionResponse<OutContractDTO>>
    {
        public CreateContractCommand() : base()
        {
        }

        public string AgentContractCode { get; set; }
        public string FiberNodeInfo { get; set; }
        public bool IsAutomaticGenerateReceipt { get; set; } = true;
        public string CustomerCareStaffUserId { get; set; }

    }
}
