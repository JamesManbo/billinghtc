using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class ApplicationUserIndustryRepository : CrudRepository<ApplicationUserIndustry, int>, IApplicationUserIndustryRepository
    {
        ApplicationUserDbContext _context;
        public ApplicationUserIndustryRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public async Task AddMapUsersIndustry(List<int> userIds, int industryId)
        {
            for (int i = 0; i < userIds.Count; i++)
            {
                var lstExist = _context.ApplicationUserIndustries.Where(m => m.UserId == userIds[i] && m.IndustryId == industryId
                                                                        && m.IsDeleted == false && m.IsActive == true).ToList();
                if (lstExist != null && lstExist.Count > 0)
                {
                    continue;
                }
                ApplicationUserIndustry industryModel = new ApplicationUserIndustry();
                industryModel.UserId = userIds[i];
                industryModel.IndustryId = industryId;
                industryModel.CreatedDate = DateTime.Now;

                _context.Add(industryModel);

            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllMapIndustryByUserId(int userId)
        {
            var lstExist = _context.ApplicationUserIndustries.Where(m => m.UserId == userId && m.IsDeleted == false && m.IsActive == true).ToList();
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

        public async Task DeleteMapIndustryByUserIdAndIndustryId(int userId, int industryId)
        {
            var lstExist = _context.ApplicationUserIndustries.Where(m => m.UserId == userId && m.IndustryId == industryId
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
