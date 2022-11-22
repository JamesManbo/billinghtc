using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface ITaxCategoryQueries : IQueryRepository
    {
        IPagedList<TaxCategoryDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        IEnumerable<TaxCategoryDTO> GetAll(RequestFilterModel filterModel = null);
        IEnumerable<TaxCategoryDTO> GetByIds(int[] ids);
        TaxCategoryDTO Find(int id);
    }
    public class TaxCategoriesQueries : QueryRepository<TaxCategory, int>, ITaxCategoryQueries
    {
        public TaxCategoriesQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {
        }

        public IPagedList<TaxCategoryDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<TaxCategoryDTO>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.TaxName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<TaxCategoryDTO> GetAll(RequestFilterModel filterModel = null)
        {
            if (filterModel != null)
            {
                filterModel.Paging = false;
            }
            else
            {
                filterModel = new RequestFilterModel()
                {
                    Paging = false
                };
            }
            var dapperExecution = BuildByTemplate<TaxCategoryDTO>(filterModel);
            return dapperExecution.ExecuteQuery();
        }

        public TaxCategoryDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<TaxCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<TaxCategoryDTO> GetByIds(int[] ids)
        {
            var dapperExecution = BuildByTemplate<TaxCategoryDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            return dapperExecution.ExecuteQuery();
        }
    }
}
