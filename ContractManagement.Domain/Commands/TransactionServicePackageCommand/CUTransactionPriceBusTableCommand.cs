using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUTransactionPriceBusTableCommand
        : DeploymentChannelPriceBusTableCommand
    {
        public int? ContractPbtId { get; set; }
        public int? TransactionServicePackageId { get; set; }
    }
}
