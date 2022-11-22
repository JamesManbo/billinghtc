using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IUserUserGroupRepository : ICrudRepository<ApplicationUserUserGroup, int>
    {
        Task DeleteAllMapGroupUserByUserId(int userId);
        Task DeleteMapGroupUserByUserIdAndGroupId(int userId, int groupId);
        Task AddMapUsersUserGroup(List<int> userIds, int groupId);
    }
}
