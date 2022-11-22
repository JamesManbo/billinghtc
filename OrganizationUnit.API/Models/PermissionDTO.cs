using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Models
{
    public class PermissionDTO
    {
        public int PermissionSetId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public string Description { get; set; }
    }
}
