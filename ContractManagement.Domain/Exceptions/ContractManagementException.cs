using System;

namespace ContractManagement.Domain.Exceptions
{
    public class ContractManagementException : Exception
    {
        public ContractManagementException()
        { }

        public ContractManagementException(string message)
            : base(message)
        { }

        public ContractManagementException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
