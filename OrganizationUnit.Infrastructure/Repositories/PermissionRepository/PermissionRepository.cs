using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public class PermissionRepository : CrudRepository<Permission, int>, IPermissionRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public PermissionRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public bool CheckExitPermissionName(string name, int id)
        {
            var lstNamePermission = _organizationUnitDbContext.Permissions.Where(x => x.PermissionName == name.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNamePermission == 0) // không tồn tại tên permission
            {
                return true;
            }
            else if (id > 0 && lstNamePermission == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckExitPermissionCode(string code, int id)
        {
            var lstCodePermission = _organizationUnitDbContext.Permissions.Where(x => x.PermissionCode == code.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstCodePermission == 0) // không tồn tại mã permission
            {
                return true;
            }
            else if (id > 0 && lstCodePermission == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
