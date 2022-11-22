using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.Models.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IPermissionQueryRepository : IQueryRepository
    {
        IPagedList<PermissionDTO> GetList(RequestFilterModel filterModel);
    }

    public class PermissionQueryRepository : QueryRepository<Permission, int>, IPermissionQueryRepository
    {
        public PermissionQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public IPagedList<PermissionDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<PermissionDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
