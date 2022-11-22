using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public interface IRolePermissionRepository : ICrudRepository<RolePermission, int>
    {
        RolePermission GetEntityByRoleIdAndPermissionId(int roleId, int permissionId);
    }
}
