using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemUserIdentity.API.Models
{
    public class UserIdentityResult
    {
        private static readonly UserIdentityResult _success = new UserIdentityResult { Succeeded = true };
        private List<UserIdentityError> _errors = new List<UserIdentityError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="UserIdentityError"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="UserIdentityError"/>s.</value>
        public IEnumerable<UserIdentityError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="UserIdentityResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="UserIdentityResult"/> indicating a successful operation.</returns>
        public static UserIdentityResult Success => _success;

        /// <summary>
        /// Creates an <see cref="UserIdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="UserIdentityError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="UserIdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static UserIdentityResult Failed(params UserIdentityError[] errors)
        {
            var result = new UserIdentityResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="UserIdentityResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="UserIdentityResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
