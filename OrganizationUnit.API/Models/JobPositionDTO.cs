using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Models
{
    public class JobPositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? IsManager { get; set; }
        public int? JobTitleId { get; set; }
    }
}
