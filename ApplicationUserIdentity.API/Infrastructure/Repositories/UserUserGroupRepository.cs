using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class UserUserGroupRepository : CrudRepository<ApplicationUserUserGroup, int>, IUserUserGroupRepository
    {
        ApplicationUserDbContext _context;
        public UserUserGroupRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public async Task AddMapUsersUserGroup(List<int> userIds, int groupId)
        {
            for(int i =0; i < userIds.Count; i++)
            {
                var lstExist = _context.ApplicationUserUserGroups.Where(m => m.UserId == userIds[i] && m.GroupId == groupId 
                                                                        && m.IsDeleted==false && m.IsActive==true).ToList();
                if (lstExist != null && lstExist.Count > 0)
                {
                    continue;
                }
                ApplicationUserUserGroup groupModel = new ApplicationUserUserGroup();
                groupModel.UserId = userIds[i];
                groupModel.GroupId = groupId;
                groupModel.CreatedDate = DateTime.Now;

                _context.Add(groupModel);
                
            }
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAllMapGroupUserByUserId(int userId)
        {
            var lstExist = _context.ApplicationUserUserGroups.Where(m => m.UserId == userId && m.IsDeleted == false && m.IsActive == true).ToList();
            if(lstExist!=null && lstExist.Count > 0)
            {
                for(int i=0; i< lstExist.Count; i++)
                {
                    var ob = lstExist.ElementAt(i);
                    ob.IsDeleted = true;
                    _context.Update(ob);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMapGroupUserByUserIdAndGroupId(int userId, int groupId)
        {
            var lstExist = _context.ApplicationUserUserGroups.Where(m => m.UserId == userId && m.GroupId== groupId
                                                                    && m.IsDeleted == false && m.IsActive == true).ToList();
            if (lstExist != null && lstExist.Count > 0)
            {
                for (int i = 0; i < lstExist.Count; i++)
                {
                    var ob = lstExist.ElementAt(i);
                    ob.IsDeleted = true;
                    _context.Update(ob);
                    
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
