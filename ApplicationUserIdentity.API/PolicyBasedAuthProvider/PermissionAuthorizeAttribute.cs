using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationUserIdentity.API.PolicyBasedAuthProvider
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private const string POLICY_PREFIX = "PERMISSION_";

        public PermissionAuthorizeAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public string PermissionCode
        {
            set => Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
