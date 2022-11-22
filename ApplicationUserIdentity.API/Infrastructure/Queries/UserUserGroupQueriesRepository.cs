using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IUserUserGroupQueriesRepository : IQueryRepository    
    {
    }
    public class UserUserGroupQueriesRepository : QueryRepository<ApplicationUserUserGroup, int>, IUserUserGroupQueriesRepository
    {
        public UserUserGroupQueriesRepository(ApplicationUserDbContext applicationUserDbContext) : base(applicationUserDbContext)
        {
        }
    }
}
