using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public interface IUserRoleRepository : ICrudRepository<UserRole, int>
    {
        
    }
}
