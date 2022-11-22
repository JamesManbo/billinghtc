using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    public class TemporaryPayingContract
    {
        public int OutContractId { get; set; }
        public int ServicePackageId { get; set; }
    }
}
