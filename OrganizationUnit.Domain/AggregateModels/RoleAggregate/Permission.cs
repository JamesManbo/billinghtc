using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OrganizationUnit.Domain.Seed;

namespace OrganizationUnit.Domain.AggregateModels.RoleAggregate
{
    [Table("Permissions")]
    public class Permission : Entity
    {
        public int PermissionSetId { get; set; }
        [StringLength(256)]
        public string PermissionName { get; set; }
        [Required(ErrorMessage = "Mã quyền là bắt buộc")]
        [StringLength(256)]
        public string PermissionCode { get; set; }
        [StringLength(256)]
        public string PermissionPage { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }
    }
}
