using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ProjectAggregate
{
    
    [Table("ProjectTechnicians")]
    public class ProjectTechnician : Entity
    {
        public int ProjectId { get; set; }
        public string UserTechnicianId { get; set; }
        public string TechnicianName { get; set; }

    }
}
