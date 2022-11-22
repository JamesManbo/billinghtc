using System;

namespace ContractManagement.Domain.Exceptions
{
    public class ContractDomainException : Exception
    {
        public ContractDomainException()
        { }

        public ContractDomainException(string message)
            : base(message)
        { }

        public ContractDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
