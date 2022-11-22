using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Configs;
using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Configs.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class UserRepository : CrudRepository<ApplicationUser, int>, IUserRepository
    {
        private readonly ApplicationUserDbContext _applicationUserDbContext;
        private readonly TokenProvideOptions _tokenProvideOptions;

        public UserRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper, IOptions<TokenProvideOptions> toikenProviderOptions) : base(
            context,
            configAndMapper)
        {
            _applicationUserDbContext = context;
            _tokenProvideOptions = toikenProviderOptions.Value;
        }

        public async Task<ApplicationUser> FindByIdentityGuidAsync(string identityGuid)
        {
            var applicationUser = await _applicationUserDbContext.ApplicationUsers.FirstOrDefaultAsync(e =>
                e.IdentityGuid.Equals(identityGuid) && !e.IsDeleted);
            return applicationUser;
        }

        public async Task<ApplicationUser> FindByUserIdentityGuidAsync(string userIdentityGuid)
        {
            var applicationUser = await _applicationUserDbContext.ApplicationUsers.FirstOrDefaultAsync(e =>
                e.UserIdentityGuid.Equals(userIdentityGuid) && !e.IsDeleted);
            return applicationUser;
        }
        public bool CheckExitUserName(string userName, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => userName.Equals(x.UserName, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0)));
        }

        public bool CheckExitUserCode(string userCode, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(userCode)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => userCode.Equals(x.CustomerCode, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false && ((x.Id != id && id != 0) || id == 0));
        }

        public bool CheckExitMobile(string mobilePhone, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(mobilePhone)) return false;

            var mobilePhones = mobilePhone.Split(",");
            return mobilePhones.Any(a => _applicationUserDbContext.ApplicationUsers.Any(x => x.MobilePhoneNo.Contains(a) && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0))));
        }

        public bool CheckExitFaxNo(string faxNo, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(faxNo)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => x.FaxNo == faxNo.Trim() && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0)));
        }

        public bool CheckExitEmail(string email, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var emails = email.Split(",");
            return emails.Any(a => _applicationUserDbContext.ApplicationUsers.Any(x => x.Email.Contains(a) && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0))));
        }

        public bool CheckExitIdNo(string idNo, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(idNo)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => x.IdNo == idNo.Trim() && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0)));
        }

        public bool CheckExitTaxIdNo(string taxIdNo, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(taxIdNo)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => x.TaxIdNo == taxIdNo.Trim() && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0)));
        }

        public bool CheckExitBusinessRegCertificate(string businessRegCertificate, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(businessRegCertificate)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => x.BusinessRegCertificate == businessRegCertificate.Trim() && x.IsDeleted == false && ((x.Id != id && id != 0) || (id == 0)));
        }

        public bool CheckExitPartner(string userIdentityGuid, int id = 0)
        {
            if (string.IsNullOrEmpty(userIdentityGuid)) return false;

            return _applicationUserDbContext.ApplicationUsers
                .Any(x => x.UserIdentityGuid == userIdentityGuid && x.IsDeleted == false && x.Id != id);
        }

        public async Task<ApplicationUser> FindByUserNameAsync(string userName)
        {
            var user = await _applicationUserDbContext.ApplicationUsers.FirstOrDefaultAsync(e =>
                e.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && !e.IsDeleted);
            return user;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(GlobalClaimsTypes.LocalId, user.Id.ToString()),
                new Claim(GlobalClaimsTypes.UniversalId, user.IdentityGuid.ToString()),
            });

            var customerPermissions = CustomerFixedPermissions();
            var permissionCodes = string.Join(',', customerPermissions);
            claimsIdentity.AddClaim(new Claim(GlobalClaimsTypes.Permissions, Compress(permissionCodes)));

            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = _tokenProvideOptions.JWTIssuer,
                NotBefore = now,
                Expires = now.Add(_tokenProvideOptions.Expiration),
                SigningCredentials = _tokenProvideOptions.SigningCredentials,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string[] CustomerFixedPermissions()
        {
            return new string[]
            {
                "VIEW_OUT_CONTRACT",
                "VIEWLIST_RECEIPT_VOUCHER",
                "ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT"
            };
        }

        private string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }
    }
}