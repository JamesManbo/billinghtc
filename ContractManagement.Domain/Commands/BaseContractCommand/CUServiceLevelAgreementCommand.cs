using ContractManagement.Domain.Commands.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.BaseContractCommand
{
    public class CUServiceLevelAgreementCommand 
        : ServiceLevelAgreementCommandAbstraction, IRequest
    {
        public int OutContractId { get; set; }
        public int? OutContractServicePackageId { get; set; }
    }
}
