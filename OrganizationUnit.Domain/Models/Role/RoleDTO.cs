using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.Role
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
