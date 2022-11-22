using ContractManagement.Domain.Commands.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.ServicePackageCommand
{
    public class CreateAppliedPromotionCommand 
        : AppliedPromotionCommandAbstraction, IRequest
    {
        public int PromotionForContractId { get; set; }
        public int OutContractServicePackageId { get; set; }
    }
}
