using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DebtManagement.API.PolicyBasedAuthProvider
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionCode { get; private set; }

        public PermissionRequirement(string permissionCode)
        {
            PermissionCode = permissionCode;
        }
    }
}