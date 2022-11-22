using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrganizationUnit.API.Models;

namespace OrganizationUnit.API.Infrastructure.Helpers.PasswordVerification
{
    public class PasswordValidator<TUser>
    {
        private readonly PasswordOptions _passwordOptions;
        /// <summary>
        /// Constructions a new instance of <see cref="PasswordValidator{TUser}"/>.
        /// </summary>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> to retrieve error text from.</param>
        public PasswordValidator(PasswordOptions passwordOptions, IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
            _passwordOptions = passwordOptions;
        }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to supply error text.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to supply error text.</value>
        public IdentityErrorDescriber Describer { get; private set; }

        /// <summary>
        /// Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> to retrieve the <paramref name="user"/> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task<UserIdentityResult> ValidateAsync(TUser user, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var errors = new List<UserIdentityError>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < _passwordOptions.RequiredLength)
            {
                errors.Add(Describer.PasswordTooShort(_passwordOptions.RequiredLength));
            }
            if (_passwordOptions.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            {
                errors.Add(Describer.PasswordRequiresNonAlphanumeric());
            }
            if (_passwordOptions.RequireDigit && !password.Any(IsDigit))
            {
                errors.Add(Describer.PasswordRequiresDigit());
            }
            if (_passwordOptions.RequireLowercase && !password.Any(IsLower))
            {
                errors.Add(Describer.PasswordRequiresLower());
            }
            if (_passwordOptions.RequireUppercase && !password.Any(IsUpper))
            {
                errors.Add(Describer.PasswordRequiresUpper());
            }
            if (_passwordOptions.RequiredUniqueChars >= 1 && password.Distinct().Count() < _passwordOptions.RequiredUniqueChars)
            {
                errors.Add(Describer.PasswordRequiresUniqueChars(_passwordOptions.RequiredUniqueChars));
            }
            return
                Task.FromResult(errors.Count == 0
                    ? UserIdentityResult.Success
                    : UserIdentityResult.Failed(errors.ToArray()));
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a digit.
        /// </summary>
        /// <param name="c">The character to check if it is a digit.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        public virtual bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is a lower case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is a lower case ASCII letter.</param>
        /// <returns>True if the character is a lower case ASCII letter, otherwise false.</returns>
        public virtual bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an upper case ASCII letter.
        /// </summary>
        /// <param name="c">The character to check if it is an upper case ASCII letter.</param>
        /// <returns>True if the character is an upper case ASCII letter, otherwise false.</returns>
        public virtual bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        /// <summary>
        /// Returns a flag indicating whether the supplied character is an ASCII letter or digit.
        /// </summary>
        /// <param name="c">The character to check if it is an ASCII letter or digit.</param>
        /// <returns>True if the character is an ASCII letter or digit, otherwise false.</returns>
        public virtual bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}
