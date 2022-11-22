using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemUserIdentity.API.Infrastructure.Helpers;
using SystemUserIdentity.API.Infrastructure.Helpers.PasswordVerification;
using SystemUserIdentity.API.Models;
using Global.Configs.Authentication;
using Global.Models.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Infrastructure.Repositories;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Domain.Models.User;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication;
using Novell.Directory.Ldap;
using Microsoft.Extensions.Configuration;
//using LdapForNet;

namespace SystemUserIdentity.API.Infrastructure.Services
{
    public class EFAuthenticationService : IAuthenticationService<User>
    {
        private readonly IUserRepository _userIdentityRepository;
        private readonly IUserQueries _userQueries;
        private readonly IRoleQueries _roleQueries;
        private readonly IConfiguration _config;

        private readonly ILogger<EFAuthenticationService> _logger;
        private readonly PasswordOptions _passwordOptions;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private readonly TokenProvideOptions _tokenProvideOptions;
        public IdentityErrorDescriber Describer { get; private set; }

        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// The <see cref="IPasswordHasher{User}"/> used to hash passwords.
        /// </summary>
        public IPasswordHasher<User> PasswordHasher { get; set; }

        public PasswordValidator<User> PasswordValidator { get; set; }

        private static ILdapConnection _conn;

        public EFAuthenticationService(IUserRepository userIdentityRepository,
            IUserQueries userQueries,
            IPasswordHasher<User> passwordHasher,
            ILogger<EFAuthenticationService> logger,
            IOptions<PasswordOptions> passwordOptions,
            IOptions<TokenProvideOptions> toikenProviderOptions,
            IRoleQueries roleQueries,
            IConfiguration config)
        {
            _userIdentityRepository = userIdentityRepository;
            _userQueries = userQueries;
            _logger = logger;
            _passwordOptions = passwordOptions.Value;
            _tokenProvideOptions = toikenProviderOptions.Value;
            PasswordHasher = passwordHasher;
            PasswordValidator = new PasswordValidator<User>(_passwordOptions);
            Describer = new IdentityErrorDescriber();
            this._roleQueries = roleQueries;
            _config = config;
        }

        public string GenerateJwtToken(string userName)
        {
            var user = _userQueries.FindByUserName(userName);
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(GlobalClaimsTypes.LocalId, user.Id.ToString()),
                new Claim(GlobalClaimsTypes.UniversalId, user.IdentityGuid.ToString()),
            });

            if (user.OrganizationUnitPaths?.Count > 0)
            {
                claimsIdentity.AddClaims(
                    user.OrganizationUnitPaths.Select(p => new Claim(GlobalClaimsTypes.OrganizationPath, p)));
            }

            if (user.MngOrganizationUnitPaths?.Count > 0)
            {
                claimsIdentity.AddClaims(
                    user.MngOrganizationUnitPaths.Select(p => new Claim(GlobalClaimsTypes.ManagerOrganizationPath, p)));
            }

            if (user.OrganizationUnits?.Count > 0)
            {
                claimsIdentity.AddClaims(
                    user.OrganizationUnits.Select(p => new Claim(GlobalClaimsTypes.Organization, p)));
            }

            var userPermissions = _userQueries.GetPermissionsOfUser(user.Id);
            if (userPermissions.Any())
            {
                var permissionCodes = string.Join(',', userPermissions.Select(p => p.PermissionCode));
                claimsIdentity.AddClaim(new Claim(GlobalClaimsTypes.Permissions, Compress(permissionCodes)));
            }

            var userRoles = _roleQueries.GetRolesOfUser(user.Id);
            if (userRoles != null && userRoles.Any())
            {
                var roleCodes = userRoles.Select(c => c.RoleCode).ToArray();
                claimsIdentity.AddClaims(roleCodes.Select(p => new Claim(ClaimTypes.Role, p)));
            }

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

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="password"/> is valid for the
        /// specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password to validate</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing true if
        /// the specified <paramref name="password" /> matches the one store for the <paramref name="user"/>,
        /// otherwise false.</returns>
        private async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null)
            {
                return false;
            }

            var result = PasswordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                await UpdatePasswordHash(user, password, validatePassword: false);
            }

            var success = result != PasswordVerificationResult.Failed;
            if (!success)
            {
                _logger.LogWarning(0, "Invalid password for user {userId}.", user.Id);
            }

            return success;
        }

        /// <summary>
        /// Updates a user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="validatePassword">Whether to validate the password.</param>
        /// <returns>Whether the password has was successfully updated.</returns>
        private async Task<UserIdentityResult> UpdatePasswordHash(User user, string newPassword,
            bool validatePassword = true)
        {
            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }

            var hash = newPassword != null ? PasswordHasher.HashPassword(newPassword) : null;
            await _userIdentityRepository.SetPasswordHashAsync(user, hash, CancellationToken);
            await _userIdentityRepository.SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
            return UserIdentityResult.Success;
        }

        private static string NewSecurityStamp()
        {
            byte[] bytes = new byte[20];
            _rng.GetBytes(bytes);
            return Base32.ToBase32(bytes);
        }

        /// <summary>
        /// Should return <see cref="Microsoft.AspNetCore.Identity.IdentityResult.Success"/> if validation is successful. This is
        /// called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> representing whether validation was successful.</returns>
        protected async Task<UserIdentityResult> ValidatePasswordAsync(User user, string password)
        {
            var errors = new List<UserIdentityError>();

            var result = await PasswordValidator.ValidateAsync(user, password);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors);
            }

            if (errors.Count > 0)
            {
                _logger.LogWarning(14, "User {userId} password validation failed: {errors}.", user.Id,
                    string.Join(";", errors.Select(e => e.Code)));
                return UserIdentityResult.Failed(errors.ToArray());
            }

            return UserIdentityResult.Success;
        }

        public Task<User> FindByUserNameAsync(string userName)
        {
            return _userIdentityRepository.FindByUserNameAsync(userName);
        }

        public string EncryptPassword(string rawPassword)
        {
            return PasswordHasher.HashPassword(rawPassword);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure)
        {
            var user = await _userIdentityRepository.FindByUserNameAsync(userName);
            if (user == null || !user.IsActive)
            {
                return SignInResult.UserNotExisted;
            }

            if (user.IsLock)
            {
                return SignInResult.LockedOut;
            }

            return await PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public async Task<SignInResult> PasswordSignInAsync(User user, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var attempt = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return attempt;
        }

        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        /// <returns></returns>
        public async Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
        {
            //return SignInResult.Success;
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var error = await PreSignInCheck(user);
            if (error != null)
            {
                return error;
            }

            if (await CheckPasswordAsync(user, password))
            {
                return SignInResult.Success;
            }

            _logger.LogWarning(2, "User {userId} failed to provide the correct password.", user.Id);
            return SignInResult.Failed;
        }

        /// <summary>
        /// Used to ensure that a user is allowed to sign in.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Null if the user should be allowed to sign in, otherwise the SignInResult why they should be denied.</returns>
        protected virtual async Task<SignInResult> PreSignInCheck(User user)
        {
            if (!await CanSignInAsync(user))
            {
                return SignInResult.NotAllowed;
            }

            if (user.IsLock)
            {
                return SignInResult.LockedOut;
            }

            return null;
        }

        /// <summary>
        /// Returns a flag indicating whether the specified user can sign in.
        /// </summary>
        /// <param name="user">The user whose sign-in status should be returned.</param>
        /// <returns>
        /// The task object representing the asynchronous operation, containing a flag that is true
        /// if the specified user can sign-in, otherwise false.
        /// </returns>
        public virtual async Task<bool> CanSignInAsync(User user)
        {
            //if (Options.SignIn.RequireConfirmedEmail && !(await UserManager.IsEmailConfirmedAsync(user)))
            //{
            //    Logger.LogWarning(0, "User {userId} cannot sign in without a confirmed email.", await UserManager.GetUserIdAsync(user));
            //    return false;
            //}
            //if (Options.SignIn.RequireConfirmedPhoneNumber && !(await UserManager.IsPhoneNumberConfirmedAsync(user)))
            //{
            //    Logger.LogWarning(1, "User {userId} cannot sign in without a confirmed phone number.", await UserManager.GetUserIdAsync(user));
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with given password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserIdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<UserIdentityResult> CreateAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var result = await UpdatePasswordHash(user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            return await CreateAsync(user);
        }

        public async Task<UserIdentityResult> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = await _userIdentityRepository.FindByUserNameAsync(userName);
            if (user == null)
            {
                return UserIdentityResult.Failed();
            }

            if (!await CheckPasswordAsync(user, oldPassword))
            {
                return UserIdentityResult.Failed(Describer.PasswordMismatch());
            }

            var result = PasswordHasher.VerifyHashedPassword(user, user.Password, oldPassword);
            if (result == PasswordVerificationResult.SuccessRehashNeeded ||
                result == PasswordVerificationResult.Success)
            {
                var updateResponse = await UpdatePasswordHash(user, newPassword, validatePassword: false);
                if (updateResponse.Succeeded)
                {
                    var updateUserResp = await _userIdentityRepository.UpdateAndSave(user);
                    return updateUserResp.IsSuccess ? UserIdentityResult.Success : UserIdentityResult.Failed();
                }
            }

            return UserIdentityResult.Failed();
        }

        public async Task<UserIdentityResult> ChangePasswordUser(string userName, string newPassword)
        {
            var user = await _userIdentityRepository.FindByUserNameAsync(userName);
            if (user == null)
            {
                return UserIdentityResult.Failed();
            }

            var updateResponse = await UpdatePasswordHash(user, newPassword, validatePassword: false);
            if (updateResponse.Succeeded)
            {
                var updateUserResp = await _userIdentityRepository.UpdateAndSave(user);
                return updateUserResp.IsSuccess ? UserIdentityResult.Success : UserIdentityResult.Failed();
            }

            return UserIdentityResult.Failed();
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with no password,
        /// as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="UserIdentityResult"/>
        /// of the operation.
        /// </returns>
        public async Task<UserIdentityResult> CreateAsync(User user)
        {
            await UpdateSecurityStampInternal(user);
            var result = await ValidateUserAsync(user);

            if (!result.Succeeded)
            {
                return result;
            }

            user.IdentityGuid = Guid.NewGuid().ToString();
            user.UserName = NormalizeUserName(user.UserName);
            user.Code = string.IsNullOrWhiteSpace(user.Code) ? user.UserName : user.Code;

            var persistentResponse = await _userIdentityRepository.CreateAndSave(user);

            if (persistentResponse.IsSuccess)
            {
                return UserIdentityResult.Success;
            }

            return UserIdentityResult.Failed(Describer.PersistentError(persistentResponse.Message));
        }

        /// <summary>
        /// Should return <see cref="Microsoft.AspNetCore.Identity.IdentityResult.Success"/> if validation is successful. This is
        /// called before saving the user via Create or Update.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>A <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> representing whether validation was successful.</returns>
        protected async Task<UserIdentityResult> ValidateUserAsync(User user)
        {
            var errors = new List<UserIdentityError>();
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(Describer.InvalidUserName(user.UserName));
            }
            else
            {
                var owner = await _userIdentityRepository.FindByUserNameAsync(user.UserName);
                if (owner != null &&
                    !string.Equals(owner.Id, user.Id))
                {
                    errors.Add(Describer.DuplicateUserName(user.UserName));
                }
            }

            if (errors.Count > 0)
            {
                _logger.LogWarning(13, "User {userId} validation failed: {errors}.", user.Id,
                    string.Join(";", errors.Select(e => e.Code)));
                return UserIdentityResult.Failed(errors.ToArray());
            }

            return UserIdentityResult.Success;
        }

        // Update the security stamp if the store supports it
        private async Task UpdateSecurityStampInternal(User user)
        {
            await _userIdentityRepository.SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
        }

        public virtual string NormalizeUserName(string key)
        {
            return key?.Normalize().ToUpperInvariant();
        }

        public UserDTO FindById(string id)
        {
            return _userQueries.FindById(id);
        }

        public async Task<bool> LoginByAD(string userName, string password)
        {
            ////ktra người dùng đã có trên db chưa
            //var user = FindByUserNameAsync(userName);
            //if (user.Result != null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return await this.GetUsersAllDomain(userName, password);
            //}

            return await this.GetUsersAllDomain(userName, password);

        }

        public async Task<bool> GetUsersAllDomain(string userName, string password)
        {
            var host = _config.GetSection("ADInfomation").GetSection("host").Value;
            var domainName = _config.GetSection("ADInfomation").GetSection("domainName").Value;
            string userDn = $"{userName}@{domainName}";

            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(host, LdapConnection.DefaultPort);
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                    {
                        var filter = $"(&(objectClass=user))";
                        LdapSearchResults searchResults = (LdapSearchResults)connection.Search(
                            "ou=htc-itc,dc=htc-itc,dc=local",//You can use String.Empty for all domain search. This is example about users
                            LdapConnection.ScopeSub,//Use SUB
                            filter,// Example of filtering with *. You can use String.Empty to query without filtering
                            null, // no specified attributes
                            false // return attr and value
                            );

                        while (searchResults.HasMore())
                        {
                            var synchronizeUsers = new List<User>();
                            var searchedUsers = searchResults.ToList();
                            if (searchedUsers.Count() > 0)
                            {
                                foreach (var user in searchedUsers)
                                {
                                    try
                                    {
                                        var addUser = new User
                                        {
                                            IdentityGuid = Guid.NewGuid().ToString(),
                                            AccountingCustomerCode = "",
                                            UserName = user.GetAttribute("sAMAccountName").StringValue,
                                            Code = ""
                                        };
                                        addUser.FirstName = user.GetAttribute("givenName")?.StringValue;
                                        addUser.LastName = user.GetAttribute("sn")?.StringValue;
                                        addUser.FullName = $"{addUser.LastName} {addUser.FirstName}";
                                        addUser.ShortName = user.GetAttribute("givenName")?.StringValue;
                                        addUser.Email = user.GetAttribute("mail")?.StringValue;
                                        addUser.Password = "";
                                        addUser.IsLock = false;
                                        addUser.IsEnterprise = false;
                                        addUser.IsCustomer = false;
                                        addUser.IsCustomerInternational = false;

                                        synchronizeUsers.Add(addUser);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError("AD Integration reading user error: {0}", ex);
                                        continue;
                                    }
                                }
                            }

                            if (synchronizeUsers.Count() > 0)
                            {
                                try
                                {
                                    foreach (var user in synchronizeUsers)
                                    {
                                        if (_userIdentityRepository.CheckExitUserName(user.UserName, user.Id))
                                        {
                                            await UpdatePasswordHash(user, user.Password, false);
                                            await CreateAsync(user);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError("AD Integration insert synchronize users error: {0}", ex);
                                }
                            }
                            return true;
                        }
                    }
                }
                return true;
            }
            catch (LdapException ex)
            {
                _logger.LogError("AD Integration error: {0}", ex);
                // Log exception
                return false;
            }
        }
    }
}