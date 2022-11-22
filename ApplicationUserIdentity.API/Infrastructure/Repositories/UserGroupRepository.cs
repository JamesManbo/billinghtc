using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class UserGroupRepository : ApplicationUserBaseRepository<ApplicationUserGroup, int>, IUserGroupRepository
    {
        public UserGroupRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
