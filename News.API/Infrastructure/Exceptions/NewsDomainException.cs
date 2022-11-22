using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Exceptions
{
    public class NewsDomainException : Exception
    {
        public NewsDomainException()
        {
        }

        protected NewsDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NewsDomainException(string message) : base(message)
        {
        }

        public NewsDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
