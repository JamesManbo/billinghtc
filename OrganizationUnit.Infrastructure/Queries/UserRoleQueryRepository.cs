using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Models.UserRole;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IUserRoleQueryRepository : IQueryRepository
    {
        IPagedList<UserRoleDto> GetList(RequestFilterModel filterModel);
    }

    public class UserRoleQueryRepository : QueryRepository<UserRole, int>, IUserRoleQueryRepository
    {
        public UserRoleQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public IPagedList<UserRoleDto> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<UserRoleDto>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }
    }
}
