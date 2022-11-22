using System.ComponentModel.DataAnnotations.Schema;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ServiceGroups")]
    public class ServiceGroup : Entity
    {
        public string GroupCode { get; set; }
        
        public string GroupName { get; set; }
    }
}
