using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Models;

namespace SystemUserIdentity.API.Infrastructure.Helpers.PasswordVerification
{
    public class IdentityErrorDescriber
    {
        /// <summary>
        /// Returns the default <see cref="UserIdentityError"/>.
        /// </summary>
        /// <returns>The default <see cref="UserIdentityError"/>.</returns>
        public virtual UserIdentityError DefaultError()
        {
            return new UserIdentityError
            {
                Code = nameof(DefaultError),
                Description = PasswordVerificationErrorResources.DefaultError
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a concurrency failure.</returns>
        public virtual UserIdentityError ConcurrencyFailure()
        {
            return new UserIdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description =  PasswordVerificationErrorResources.ConcurrencyFailure
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password mismatch.</returns>
        public virtual UserIdentityError PasswordMismatch()
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = PasswordVerificationErrorResources.PasswordMismatch
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating an invalid token.</returns>
        public virtual UserIdentityError InvalidToken()
        {
            return new UserIdentityError
            {
                Code = nameof(InvalidToken),
                Description = PasswordVerificationErrorResources.InvalidToken
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a recovery code was not redeemed.</returns>
        public virtual UserIdentityError RecoveryCodeRedemptionFailed()
        {
            return new UserIdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = PasswordVerificationErrorResources.RecoveryCodeRedemptionFailed
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating an external login is already associated with an account.</returns>
        public virtual UserIdentityError LoginAlreadyAssociated()
        {
            return new UserIdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = PasswordVerificationErrorResources.LoginAlreadyAssociated
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified user <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specified user <paramref name="userName"/> is invalid.</returns>
        public virtual UserIdentityError InvalidUserName(string userName)
        {
            return new UserIdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format(PasswordVerificationErrorResources.InvalidUserName, userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specified <paramref name="email"/> is invalid.</returns>
        public virtual UserIdentityError InvalidEmail(string email)
        {
            return new UserIdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.Format(PasswordVerificationErrorResources.InvalidEmail, email)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified <paramref name="userName"/> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specified <paramref name="userName"/> already exists.</returns>
        public virtual UserIdentityError DuplicateUserName(string userName)
        {
            return new UserIdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(PasswordVerificationErrorResources.DuplicateUserName, userName)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified <paramref name="email"/> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specified <paramref name="email"/> is already associated with an account.</returns>
        public virtual UserIdentityError DuplicateEmail(string email)
        {
            return new UserIdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format(PasswordVerificationErrorResources.DuplicateEmail, email)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified <paramref name="role"/> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specific role <paramref name="role"/> name is invalid.</returns>
        public virtual UserIdentityError InvalidRoleName(string role)
        {
            return new UserIdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = string.Format(PasswordVerificationErrorResources.InvalidRoleName, role)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating the specified <paramref name="role"/> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating the specific role <paramref name="role"/> name already exists.</returns>
        public virtual UserIdentityError DuplicateRoleName(string role)
        {
            return new UserIdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = string.Format(PasswordVerificationErrorResources.DuplicateRoleName, role)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a user already has a password.</returns>
        public virtual UserIdentityError UserAlreadyHasPassword()
        {
            return new UserIdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = PasswordVerificationErrorResources.UserAlreadyHasPassword
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating user lockout is not enabled.</returns>
        public virtual UserIdentityError UserLockoutNotEnabled()
        {
            return new UserIdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = PasswordVerificationErrorResources.UserLockoutNotEnabled
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a user is already in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating a user is already in the specified <paramref name="role"/>.</returns>
        public virtual UserIdentityError UserAlreadyInRole(string role)
        {
            return new UserIdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = string.Format(PasswordVerificationErrorResources.UserAlreadyInRole, role)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a user is not in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating a user is not in the specified <paramref name="role"/>.</returns>
        public virtual UserIdentityError UserNotInRole(string role)
        {
            return new UserIdentityError
            {
                Code = nameof(UserNotInRole),
                Description = string.Format(PasswordVerificationErrorResources.UserNotInRole, role)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.</returns>
        public virtual UserIdentityError PasswordTooShort(int length)
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format(PasswordVerificationErrorResources.PasswordTooShort, length)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.</returns>
        public virtual UserIdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = string.Format(PasswordVerificationErrorResources.PasswordRequiresUniqueChars, uniqueChars)
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public virtual UserIdentityError PasswordRequiresNonAlphanumeric()
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = PasswordVerificationErrorResources.PasswordRequiresNonAlphanumeric
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password entered does not contain a numeric character.</returns>
        public virtual UserIdentityError PasswordRequiresDigit()
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = PasswordVerificationErrorResources.PasswordRequiresDigit
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password entered does not contain a lower case letter.</returns>
        public virtual UserIdentityError PasswordRequiresLower()
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = PasswordVerificationErrorResources.PasswordRequiresLower
            };
        }

        /// <summary>
        /// Returns an <see cref="UserIdentityError"/> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="UserIdentityError"/> indicating a password entered does not contain an upper case letter.</returns>
        public virtual UserIdentityError PasswordRequiresUpper()
        {
            return new UserIdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = PasswordVerificationErrorResources.PasswordRequiresUpper
            };
        }
        
        public virtual UserIdentityError PersistentError(string persistentError)
        {
            return new UserIdentityError
            {
                Code = nameof(PersistentError),
                Description = persistentError
            };
        }
    }
}
