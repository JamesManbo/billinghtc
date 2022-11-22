using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IUserGroupQueriesRepository : IQueryRepository
    {
        IPagedList<UserGroupDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        UserGroupDTO FindById(int id);

        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
    }
    public class UserGroupQueriesRepository : QueryRepository<ApplicationUserGroup, int>, IUserGroupQueriesRepository
    {
        public UserGroupQueriesRepository(ApplicationUserDbContext applicationUserDbContext) : base(applicationUserDbContext)
        {
        }

        public UserGroupDTO FindById(int id)
        {
            var dapperExecution = BuildByTemplate<UserGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IPagedList<UserGroupDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<UserGroupDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.GroupName AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.GroupCode AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            return dapperExecution.ExecuteQuery();
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<UserGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.GroupCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<UserGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.GroupName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }
    }
}
