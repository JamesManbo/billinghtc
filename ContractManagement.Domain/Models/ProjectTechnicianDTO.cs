using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public class ProjectTechnicianDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; }
    }
}
