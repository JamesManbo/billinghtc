using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrganizationUnit.API.Infrastructure.Helpers;
using OrganizationUnit.API.Infrastructure.Helpers.PasswordVerification;
using OrganizationUnit.API.Models;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        public IdentityErrorDescriber Describer { get; private set; }
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        private readonly ILogger<UserService> _logger;

        private readonly PasswordOptions _passwordOptions;

        public IPasswordHasher<User> PasswordHasher { get; set; }
        public PasswordValidator<User> PasswordValidator { get; set; }

        public UserService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ILogger<UserService> logger,
            IOptions<PasswordOptions> passwordOptions)
        {
            _userRepository = userRepository;
            _logger = logger;
            PasswordHasher = passwordHasher;
            _passwordOptions = passwordOptions.Value;
            PasswordValidator = new PasswordValidator<User>(_passwordOptions);
            Describer = new IdentityErrorDescriber();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await Task.FromResult(_userRepository.GetByIdAsync(id).Result);
        }

        public virtual async Task<ActionResponse> CreateAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password != null)
            {
                var result = await UpdatePasswordHash(user, password);

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return await CreateAsync(user);
        }

        public async Task<ActionResponse> CreateAsync(User user)
        {
            await UpdateSecurityStampInternal(user);
            var result = await ValidateUserAsync(user);

            if (!result.IsSuccess)
            {
                return result;
            }

            user.UserName = NormalizeUserName(user.UserName);

            var persistentResponse = await _userRepository.CreateAndSave(user);

            if (persistentResponse.IsSuccess)
            {
                return persistentResponse;
            }

            return persistentResponse;
        }

        private async Task<ActionResponse> UpdatePasswordHash(User user, string newPassword,
            bool validatePassword = true)
        {
            var commandRespone = new ActionResponse();

            if (validatePassword)
            {
                var validate = await ValidatePasswordAsync(user, newPassword);
                if (!validate.Succeeded)
                {
                    var firstError = validate.Errors?.FirstOrDefault();
                    commandRespone.AddError(
                        firstError?.Description, nameof(user.Password));
                    return commandRespone;
                }
            }

            var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
            await _userRepository.SetPasswordHashAsync(user, hash, CancellationToken);
            await _userRepository.SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
            return commandRespone;
        }

        private async Task UpdateSecurityStampInternal(User user)
        {
            await _userRepository.SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
        }

        private static string NewSecurityStamp()
        {
            byte[] bytes = new byte[20];
            _rng.GetBytes(bytes);
            return Base32.ToBase32(bytes);
        }

        protected async Task<ActionResponse> ValidateUserAsync(User user)
        {
            var commandRespone = new ActionResponse();
            var errors = new List<UserIdentityError>();
            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                var owner = await _userRepository.FindByUserNameAsync(user.UserName);
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
                return commandRespone.AddError("Tên tài khoản không hợp lệ");
            }

            return commandRespone;
        }

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

        public virtual string NormalizeUserName(string key)
        {
            return key?.Normalize().ToUpperInvariant() ?? string.Empty;
        }


        public IActionResponse DeleteAndSave(int Id)
        {
            return _userRepository.DeleteAndSave(Id);
        }

        public bool IsExisted(int Id)
        {
            return _userRepository.IsExisted(Id);
        }

        public virtual async Task<ActionResponse> UpdateAsync(User user, string password)
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
            if (!result.IsSuccess)
            {
                return result;
            }

            return await UpdateAsync(user);
        }

        public virtual async Task<ActionResponse> UpdateWithoutPasswordAsync<T>(T user)
            where T : class
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _userRepository.UpdateAndSave(user);
        }

        public async Task<ActionResponse> UpdateAsync(User user)
        {
            await UpdateSecurityStampInternal(user);
            var result = await ValidateUserAsync(user);

            if (!result.IsSuccess)
            {
                return result;
            }

            return await _userRepository.UpdateAndSave(user);
        }

        public bool CheckExitUserName(string userName, int id)
        {
            if (string.IsNullOrWhiteSpace(userName)) return true;
            return _userRepository.CheckExitUserName(userName.Trim(), id);
        }

        public bool CheckExitPhoneNumber(string phoneNumber, int id)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return true;
            return _userRepository.CheckExitPhoneNumber(phoneNumber.Trim(), id);
        }

        public bool CheckExitEmail(string email, int id)
        {
            if (string.IsNullOrWhiteSpace(email)) return true;
            return _userRepository.CheckExitEmail(email.Trim(), id);
        }

        public bool CheckExitIdNumber(string idNumber, int id)
        {
            if (string.IsNullOrWhiteSpace(idNumber)) return true;
            return _userRepository.CheckExitIdNumber(idNumber.Trim(), id);
        }

        public bool CheckExitIdTaxNumber(string idNumber, int id)
        {
            if (string.IsNullOrWhiteSpace(idNumber)) return true;
            return _userRepository.CheckExitIdTaxNumber(idNumber.Trim(), id);
        }

        public bool CheckExistedCode(string code, int id)
        {
            if (string.IsNullOrWhiteSpace(code)) return true;
            return _userRepository.CheckExistedCode(code.Trim(), id);
        }
        public bool CheckExitCustomer(string applicationUserIdentityGuid, int id)
        {
            if (string.IsNullOrEmpty(applicationUserIdentityGuid)) return true;
            return _userRepository.CheckExitCustomer(applicationUserIdentityGuid, id);
        }

        public bool AddConfigUsers(int useId)
        {
            return _userRepository.AddConfigUsers(useId);
        }

    }
}