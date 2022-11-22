using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using GenericRepository;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IUserClassRepository : ICrudRepository<ApplicationUserClass, int>
    {
    }
}
