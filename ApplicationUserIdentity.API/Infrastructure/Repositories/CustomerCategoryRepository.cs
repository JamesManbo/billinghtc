using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class CustomerCategoryRepository : ApplicationUserBaseRepository<CustomerCategory, int>, ICustomerCategoryRepository
    {
        public CustomerCategoryRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
          context,
          configAndMapper)
        {
        }
    }
}
