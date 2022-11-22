using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using System.Linq;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public class RolePermissionRepository : CrudRepository<RolePermission, int>, IRolePermissionRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public RolePermissionRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public RolePermission GetEntityByRoleIdAndPermissionId(int roleId, int permissionId)
        {
            var entity = _organizationUnitDbContext.RolePermissions.FirstOrDefault(o => o.RoleId == roleId && o.PermissionId == permissionId);
            return entity;
        }
    }
}
