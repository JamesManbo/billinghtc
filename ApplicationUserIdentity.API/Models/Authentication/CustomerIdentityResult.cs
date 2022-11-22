using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.Authentication
{
    public class CustomerIdentityResult
    {
        private static readonly CustomerIdentityResult _success = new CustomerIdentityResult { Succeeded = true };
        private List<CustomerIdentityError> _errors = new List<CustomerIdentityError>();

        public bool Succeeded { get; protected set; }

        public IEnumerable<CustomerIdentityError> Errors => _errors;

        public static CustomerIdentityResult Success => _success;

        public static CustomerIdentityResult Failed(params CustomerIdentityError[] errors)
        {
            var result = new CustomerIdentityResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
