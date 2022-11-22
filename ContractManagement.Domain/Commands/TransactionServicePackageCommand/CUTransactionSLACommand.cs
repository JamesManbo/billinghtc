using ContractManagement.Domain.Commands.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.BaseContractCommand
{
    public class CUTransactionSLACommand
        : ServiceLevelAgreementCommandAbstraction, IRequest
    {
        public int? TransactionServicePackageId { get; set; }
        public int? ContractSlaId { get; set; }
    }
}
