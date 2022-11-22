using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface ICustomerTypeRepository : ICrudRepository<Models.CustomerType, int>
    {
    }
}
