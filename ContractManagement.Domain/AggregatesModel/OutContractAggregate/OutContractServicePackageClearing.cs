using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("OutContractServicePackageClearings")]
    public class OutContractServicePackageClearing : Entity
    {
        public decimal Change { get; set; }
        public bool IsUsed { get; set; }
        public int OutContractServicePackageId { get; set; }
        public bool IsDaysPlus { get; set; }
    }
}
