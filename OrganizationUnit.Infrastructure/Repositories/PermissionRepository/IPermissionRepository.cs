using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories
{
    public interface IPermissionRepository : ICrudRepository<Permission, int>
    {
        bool CheckExitPermissionName(string name, int id);
        bool CheckExitPermissionCode(string code, int id);
    }
}
