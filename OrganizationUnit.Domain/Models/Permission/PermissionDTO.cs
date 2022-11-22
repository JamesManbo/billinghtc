using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.Permission
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPage { get; set; }
        public string Description { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public bool Grant { get; set; }
        public bool Deny { get; set; }
    }
}
