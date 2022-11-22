using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Exceptions
{
    public class OrganizationDomainException : Exception
    {
        public OrganizationDomainException()
        {
        }

        public OrganizationDomainException(string? message) : base(message)
        {
        }

        public OrganizationDomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
