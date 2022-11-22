using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.JobPosition
{
    public class JobPositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool? IsManager { get; set; }
        public int? JobTitleId { get; set; }
    }
}
