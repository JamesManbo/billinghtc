using ApplicationUserIdentity.API.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IFCMTokenRepository : ICrudRepository<FCMToken, int>
    {
        Task<FCMToken> GetByToken(string token);
    }
}
