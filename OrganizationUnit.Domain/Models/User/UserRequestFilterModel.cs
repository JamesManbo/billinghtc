using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.User
{
    public class UserRequestFilterModel: RequestFilterModel
    {
        public bool IsPartner { get; set; }
        public bool? Assigned { get; set; }
        public int RoleId { get; set; }
    }
}
