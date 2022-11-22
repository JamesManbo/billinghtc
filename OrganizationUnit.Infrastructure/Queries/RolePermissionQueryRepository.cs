using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.Models.RolePermission;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IRolePermissionQueryRepository : IQueryRepository
    {
        IPagedList<RolePermissionDto> GetList(RequestFilterModel filterModel);
    }

    public class RolePermissionQueryRepository : QueryRepository<RolePermission, int>, IRolePermissionQueryRepository
    {
        public RolePermissionQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public IPagedList<RolePermissionDto> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<RolePermissionDto>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
