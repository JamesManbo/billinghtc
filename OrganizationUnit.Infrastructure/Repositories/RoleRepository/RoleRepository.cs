using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.RoleAggregate;
using System.Linq;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public class RoleRepository : CrudRepository<Role, int>, IRoleRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public RoleRepository(OrganizationUnitDbContext  organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }

        public bool CheckExitRoleName(string name, int id)
        {
            var lstNameRole = _organizationUnitDbContext.Roles.Where(x => x.RoleName == name.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNameRole == 0) // không tồn tại tên role
            {
                return true;
            }
            else if (id > 0 && lstNameRole == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckExitRoleCode(string code, int id)
        {
            var lstCodeRole = _organizationUnitDbContext.Roles.Where(x => x.RoleCode == code.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstCodeRole == 0) // không tồn tại mã role
            {
                return true;
            }
            else if (id > 0 && lstCodeRole == 0)
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
