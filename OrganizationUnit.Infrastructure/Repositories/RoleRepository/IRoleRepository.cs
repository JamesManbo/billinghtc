using GenericRepository;
using OrganizationUnit.Domain.RoleAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public interface IRoleRepository : ICrudRepository<Role, int>
    {
        bool CheckExitRoleName(string code, int id);
        bool CheckExitRoleCode(string code, int id);
    }
}
