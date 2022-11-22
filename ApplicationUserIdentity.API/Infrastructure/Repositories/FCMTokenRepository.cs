using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class FCMTokenRepository : CrudRepository<FCMToken, int>, IFCMTokenRepository
    {
        private readonly ApplicationUserDbContext _applicationUserDbContext;
        public FCMTokenRepository(ApplicationUserDbContext context, 
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _applicationUserDbContext = context;
        }

        public Task<FCMToken> GetByToken(string token)
        {
            return DbSet.FirstOrDefaultAsync(c => c.Token.Equals(token));
        }
    }
}
