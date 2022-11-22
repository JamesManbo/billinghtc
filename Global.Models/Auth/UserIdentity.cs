using Global.Configs.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Global.Models.Auth
{
    public class UserIdentity
    {
        private readonly ClaimsIdentity _claimsIdentity;
        public UserIdentity(IHttpContextAccessor httpContextAccessor)
        {
            this._claimsIdentity = httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
            DecomposeRawPermissionClaims();
        }
        public UserIdentity(HttpContext httpContext)
        {
            this._claimsIdentity = httpContext?.User?.Identity as ClaimsIdentity;
            DecomposeRawPermissionClaims();
        }

        private void DecomposeRawPermissionClaims()
        {
            var permissionsOfUser = new HashSet<string>();
            var permissionClaims = this._claimsIdentity?
                    .FindAll(GlobalClaimsTypes.Permissions)
                    ?.Select(v => v.Value)
                    ?.ToList();

            if (permissionClaims == null || permissionClaims.Count == 0) return;

            foreach (var permissions in permissionClaims)
            {
                var decompressPermissions = DecompressPermission(permissions).Split(',');
                Array.ForEach(decompressPermissions, c => permissionsOfUser.Add(c.ToUpper()));
            }

            this.Permissions = permissionsOfUser.ToArray();
        }
        public int Id => Convert.ToInt32(
            this._claimsIdentity?.FindFirst(GlobalClaimsTypes.LocalId)?.Value
        );

        public string UniversalId => this._claimsIdentity?.FindFirst(GlobalClaimsTypes.UniversalId)?.Value;
        public string UserName => this._claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string FullName => this._claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
        public string FirstName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.FullName))
                {
                    return string.Empty;
                }

                return this.FullName.Split(' ').Last();
            }
        }
        public string LastName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.FullName))
                {
                    return string.Empty;
                }

                return this.FullName.Replace(this.FirstName, "");
            }
        }
        public string MobilePhoneNo => this._claimsIdentity?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        public string Address => this._claimsIdentity?.FindFirst(ClaimTypes.StreetAddress)?.Value;
        public string Email => this._claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
        public string[] Roles => this._claimsIdentity?.FindAll(ClaimTypes.Role)?.Select(v => v.Value)?.ToArray();
        public string[] Permissions { get; set; }
        public string[] OrganizationPaths => this._claimsIdentity?.FindAll(GlobalClaimsTypes.OrganizationPath)?.Select(v => v.Value)?.ToArray();
        public string[] MngOrganizationPaths => this._claimsIdentity?.FindAll(GlobalClaimsTypes.ManagerOrganizationPath)?.Select(v => v.Value)?.ToArray();
        public string[] Organizations => this._claimsIdentity?.FindAll(GlobalClaimsTypes.Organization)?.Select(v => v.Value)?.ToArray();

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
