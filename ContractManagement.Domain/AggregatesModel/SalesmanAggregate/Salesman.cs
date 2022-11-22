using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.SalesmanAggregate
{
    [Table("Salesman")]
    public class Salesman : Entity
    {
        [StringLength(20)]
        public string IdentityGuid { get; set; }

        public string FullName { get; set; }
    }
}
