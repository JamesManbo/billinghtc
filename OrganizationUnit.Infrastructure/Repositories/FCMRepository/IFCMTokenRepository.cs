using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationUnit.Infrastructure.Repositories.FCMRepository
{
    public interface IFCMTokenRepository : ICrudRepository<FCMToken, int>
    {
        Task<FCMToken> GetByToken(string token);
    }
}
