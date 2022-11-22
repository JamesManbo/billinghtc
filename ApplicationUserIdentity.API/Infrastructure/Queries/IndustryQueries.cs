using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IIndustryQueries: IQueryRepository
    {
        IPagedList<IndustryDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        IndustryDTO FindById(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);

    }

    public class IndustryQueries : QueryRepository<Industry, int>, IIndustryQueries
    {
        public IndustryQueries(ApplicationUserDbContext applicationUserDbContext) : base(applicationUserDbContext)
        {
        }

        public IPagedList<IndustryDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<IndustryDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Code AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            return dapperExecution.ExecuteQuery();
        }

        public IndustryDTO FindById(int id)
        {
            var dapperExecution = BuildByTemplate<IndustryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<IndustryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Code = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<IndustryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Name = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }
    }
}
