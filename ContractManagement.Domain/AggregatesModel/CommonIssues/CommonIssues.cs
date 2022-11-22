using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.CommonIssues
{
    [Table("RequestSupports")]
    public class CommonIssues : Entity
    {
        public string Name { get; set; }
        
    }
}
