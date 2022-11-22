using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class CustomerStructureRepository : ApplicationUserBaseRepository<CustomerStructure, int>, ICustomerStructureRepository
    {
        public CustomerStructureRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
          context,
          configAndMapper)
        {
        }
    }
}
