using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using News.API.Models;
using News.API.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Queries
{
    public interface IArticleCategoryQueries : IQueryRepository
    {
        IPagedList<ArticleCategoryDto> GetList(RequestFilterModel filterModel);
        ArticleCategoryDto FindById(int id);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        IEnumerable<ArticleCategoryDto> GetAll(RequestFilterModel filterModel = null);
    }
    public class ArticleCategoryQueries : QueryRepository<ArticleCategory, int>, IArticleCategoryQueries
    {
        public ArticleCategoryQueries(NewsDbContext context) : base(context)
        {
        }

        public IPagedList<ArticleCategoryDto> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ArticleCategoryDto>(filterModel);

            return dapperExecution.ExecutePaginateQuery();
        }

        public ArticleCategoryDto FindById(int id)
        {
            var dapperExecution = BuildByTemplate<ArticleCategoryDto>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<ArticleCategoryDto> GetAll(RequestFilterModel filterModel = null)
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
            var dapperExecution = BuildByTemplate<ArticleCategoryDto>(filterModel);
            return dapperExecution.ExecuteQuery();
        }
    }
}
