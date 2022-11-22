using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using OrganizationUnit.Domain.Models.Role;
using OrganizationUnit.Domain.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IRoleQueries : IQueryRepository
    {
        IPagedList<RoleDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<RoleDTO> GetRolesOfUser(int userId);
    }

    public class RoleQueries : QueryRepository<Role, int>, IRoleQueries
    {
        public RoleQueries(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public IPagedList<RoleDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<RoleDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<RoleDTO> GetRolesOfUser(int userId)
        {
            var dapperExecution = BuildByTemplate<RoleDTO>();
            dapperExecution.SqlBuilder.InnerJoin("UserRoles ur ON ur.RoleId = t1.Id");
            dapperExecution.SqlBuilder.Where("ur.UserId = @userId", new { userId });
            return dapperExecution.ExecuteQuery();
        }
    }
}
