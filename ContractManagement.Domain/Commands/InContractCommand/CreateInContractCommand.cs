using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.InContractCommand
{
    public class CreateInContractCommand : CreateUpdateContractBaseCommand, IRequest<ActionResponse<InContractDTO>>
    {
        public string FiberNodeInfo { get; set; }
        public int[] ContractSharingRevenuesOutContractId { get; set; }
        public List<CUContractSharingRevenueLineCommand> ContractSharingRevenues { get; set; }
        //public List<CUInContractServiceCommand> InContractServices { get; set; }
    }
}
