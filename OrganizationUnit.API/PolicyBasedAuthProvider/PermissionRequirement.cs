using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.PolicyBasedAuthProvider
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        public string PermissionCode { get; set; }
        public PermissionRequirement(string permissionCode)
        {
            PermissionCode = permissionCode;
        }
    }
}
