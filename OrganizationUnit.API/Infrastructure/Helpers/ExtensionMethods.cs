using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.Helpers
{
    public static class ExtensionMethods
    {
        public static User PasswordNull(this User user)
        {
            user.Password = null;
            return user;
        }
    }
}
