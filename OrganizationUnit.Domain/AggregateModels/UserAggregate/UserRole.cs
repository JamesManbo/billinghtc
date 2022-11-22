using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using OrganizationUnit.Domain.RoleAggregate;
using OrganizationUnit.Domain.Seed;

namespace OrganizationUnit.Domain.AggregateModels.UserAggregate
{
    [Table("UserRoles")]
    public class UserRole : Entity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        private User _user;
        private Role _role;
    }
}
