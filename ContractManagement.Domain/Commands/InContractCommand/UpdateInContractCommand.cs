using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.InContractCommand
{
    public class UpdateInContractCommand : CreateUpdateContractBaseCommand, IRequest<ActionResponse<InContractDTO>>
    {
        public UpdateInContractCommand() : base()
        {
        }

        public string FiberNodeInfo { get; set; }
        public int[] InletChannelIds { get; set; }
    }
}
