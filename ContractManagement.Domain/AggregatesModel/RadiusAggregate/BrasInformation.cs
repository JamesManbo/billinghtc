using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.RadiusAggregate
{
    [Table("BrasInformation")]
    public class BrasInformation : Entity
    {
        public string IP { get; set; }
        public int ManualAPIPort { get; set; }
        public int? SSHPort { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? ProjectId { get; set; }
        public string Description { get; set; }
    }
}
