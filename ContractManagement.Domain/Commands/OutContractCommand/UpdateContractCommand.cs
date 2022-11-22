using System.Collections.Generic;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class UpdateContractCommand : CreateUpdateContractBaseCommand, IRequest<ActionResponse<OutContractDTO>>
    {
        public UpdateContractCommand() : base()
        {
            Equipments = new List<CUContractEquipmentCommand>();
        }

        public string AgentContractCode { get; set; }
        public string FiberNodeInfo { get; set; }
        public bool IsAutomaticGenerateReceipt { get; set; }
        public List<CUContractEquipmentCommand> Equipments { get; set; }
        public string CustomerCareStaffUserId { get; set; }
    }
}
