using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IUserGroupRepository : ICrudRepository<ApplicationUserGroup, int>
    {
    }
}
