using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DebtManagement.API.PolicyBasedAuthProvider
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ILogger<PermissionRequirement> _logger;

        public PermissionAuthorizationHandler(ILogger<PermissionRequirement> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            _logger.LogWarning("Evaluating authorization requirement for permission: {permissionCode}",
                requirement.PermissionCode);

            //Check the user's permission
            var permissionsOfUser = new HashSet<string>();
            var permissionClaims = context.User.FindAll(c => c.Type == "Permissions")
                .Select(p => p.Value);
            foreach (var permissions in permissionClaims)
            {
                var decompressPermissions = DecompressPermission(permissions).Split(',');
                Array.ForEach(decompressPermissions, c => permissionsOfUser.Add(c.ToUpper()));
            }

            if (!string.IsNullOrEmpty(requirement.PermissionCode)
                && permissionsOfUser.Contains(requirement.PermissionCode.ToUpper()))
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Do not have any permission: {permissionCode} in the user's permissions",
                    requirement.PermissionCode);
                context.Fail();
            }

            return Task.CompletedTask;
        }
        private string DecompressPermission(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}