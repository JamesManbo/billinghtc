using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IApplicationUserIndustryRepository : ICrudRepository<ApplicationUserIndustry, int>
    {
        Task DeleteAllMapIndustryByUserId(int userId);
        Task DeleteMapIndustryByUserIdAndIndustryId(int userId, int industryId);
        Task AddMapUsersIndustry(List<int> userIds, int industryId);
    }
}
