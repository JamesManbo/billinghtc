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
    public interface ICustomerTypeQueries
    {
        IPagedList<CustomerTypeDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList();
        CustomerTypeDTO Find(int id);
        CustomerTypeDTO FindByUserId(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
    }
    public class CustomerTypeQueries : QueryRepository<CustomerType, int>,ICustomerTypeQueries
    {
        public CustomerTypeQueries(ApplicationUserDbContext applicationUserDbContext) : base(applicationUserDbContext)
        {

        }
        public IPagedList<CustomerTypeDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<CustomerTypeDTO>(filterModel);

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

        public CustomerTypeDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<CustomerTypeDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public CustomerTypeDTO FindByUserId(int id)
        {
            var dapperExecution = BuildByTemplate<CustomerTypeDTO>();
            dapperExecution.SqlBuilder.Where("t1.CreatedUId = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<CustomerTypeDTO>();
            dapperExecution.SqlBuilder.Where("t1.Name = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<CustomerTypeDTO>();
            dapperExecution.SqlBuilder.Where("t1.Code = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }
    }
}
