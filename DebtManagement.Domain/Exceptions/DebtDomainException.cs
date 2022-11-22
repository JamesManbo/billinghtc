using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Exceptions
{
    public class DebtDomainException : Exception
    {
        public DebtDomainException()
        { }

        public DebtDomainException(string message)
            : base(message)
        { }

        public DebtDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
