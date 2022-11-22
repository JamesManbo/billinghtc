using ContractManagement.Domain.Commands.Abstraction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionServicePackageCommand
{
    public class CUTransactionTaxCommand: TaxValueCommandAbstraction,
        IRequest
    {
        public int TransactionId { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public int? ContractChannelTaxId { get; set; }
    }
}
