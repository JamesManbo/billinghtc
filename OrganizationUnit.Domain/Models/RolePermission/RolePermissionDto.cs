using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.RolePermission
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool Grant { get; set; }
        public bool Deny { get; set; }
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
