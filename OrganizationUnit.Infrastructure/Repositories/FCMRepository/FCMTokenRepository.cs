using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationUnit.Infrastructure.Repositories.FCMRepository
{
    public class FCMTokenRepository : CrudRepository<FCMToken, int>, IFCMTokenRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public FCMTokenRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public Task<FCMToken> GetByToken(string token)
        {
            return DbSet.FirstOrDefaultAsync(c => c.Token.Equals(token));
        }
    }
}
