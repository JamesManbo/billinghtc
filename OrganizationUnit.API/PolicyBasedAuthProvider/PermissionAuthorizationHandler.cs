using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationUnit.API.PolicyBasedAuthProvider
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement permissionRequirement)
        {
            var permissionsOfUser = new HashSet<string>();
            var permissionClaims = context.User.FindAll(c => c.Type == "Permissions")
                .Select(p => p.Value);
            foreach (var permissions in permissionClaims)
            {
                var decompressPermissions = DecompressPermission(permissions).Split(',');
                Array.ForEach(decompressPermissions, c => permissionsOfUser.Add(c.ToUpper()));
            }

            if (!string.IsNullOrEmpty(permissionRequirement.PermissionCode) 
                && permissionsOfUser.Contains(permissionRequirement.PermissionCode.ToUpper()))
            {
                context.Succeed(permissionRequirement);
            }

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
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
