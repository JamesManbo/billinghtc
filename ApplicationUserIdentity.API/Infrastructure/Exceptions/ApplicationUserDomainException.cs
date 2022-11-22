using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Exceptions
{
    public class ApplicationUserDomainException : Exception
    {
        public ApplicationUserDomainException()
        {
        }

        protected ApplicationUserDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ApplicationUserDomainException(string message) : base(message)
        {
        }

        public ApplicationUserDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
