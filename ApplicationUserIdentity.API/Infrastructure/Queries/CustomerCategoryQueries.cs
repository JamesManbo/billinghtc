using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Response;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface ICustomerCategoryQueries
    {
        IPagedList<CustomerCategoryDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList();
        CustomerCategoryDTO Find(int id);
        CustomerCategoryDTO FindByUserId(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
    }
    public class CustomerCategoryQueries : QueryRepository<CustomerCategory, int>, ICustomerCategoryQueries
    {
        public CustomerCategoryQueries(ApplicationUserDbContext applicationUserDbContext) : base(applicationUserDbContext)
        {

        }
        public IPagedList<CustomerCategoryDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<CustomerCategoryDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.Name AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Code AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            return dapperExecution.ExecuteQuery();
        }

        public CustomerCategoryDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<CustomerCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public CustomerCategoryDTO FindByUserId(int id)
        {
            var dapperExecution = BuildByTemplate<CustomerCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.CreatedUId = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<CustomerCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Name = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<CustomerCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Code = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }
    }
}
