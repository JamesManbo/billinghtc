using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public class UserRoleRepository : CrudRepository<UserRole, int>, IUserRoleRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public UserRoleRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }
    }
}
