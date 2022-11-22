using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.RoleAggregate;
using OrganizationUnit.Domain.Seed;

namespace OrganizationUnit.Domain.AggregateModels.RoleAggregate
{
    [Table("RolePermissions")]
    public class RolePermission : Entity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public bool Grant { get; set; }
        public bool Deny { get; set; }
    }
}
