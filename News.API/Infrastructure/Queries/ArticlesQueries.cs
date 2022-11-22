using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using News.API.Models;
using News.API.Models.Domain;
using News.API.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Queries
{
    public interface IArticlesQueries : IQueryRepository
    {
        IPagedList<ArticleDto> GetList(ArticleFilterModel filterModel);
        ArticleDto Find(int id);
        ArticleDto Find(string id);
    }
    public class ArticlesQueries : QueryRepository<Article, int>, IArticlesQueries
    {
        public ArticlesQueries(NewsDbContext context) : base(context)
        {
        }

        public IPagedList<ArticleDto> GetList(ArticleFilterModel filterModel)
        {
            var cache = new Dictionary<int, ArticleDto>();
            var dapperExecution = BuildByTemplate<ArticleDto>(filterModel);

            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Name");
            dapperExecution.SqlBuilder.Select("t2.FileName");
            dapperExecution.SqlBuilder.Select("t2.FilePath");
            dapperExecution.SqlBuilder.Select("t2.Size");
            dapperExecution.SqlBuilder.Select("t2.Extension");
            dapperExecution.SqlBuilder.Select("t2.RedirectLink");

            dapperExecution.SqlBuilder.LeftJoin("Pictures t2 ON t1.AvatarId = t2.Id AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("ArticleArticleCategories AS t3 ON t3.ArticleId = t1.Id AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("ArticleCategories AS t4 ON t4.Id = t3.ArticleCategoryId AND t4.IsDeleted = FALSE");

            if (filterModel.FromDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.CreatedDate >= @fromDate", new { fromDate = filterModel.FromDate.Value });
            }
            if (filterModel.ToDate.HasValue)
            {
                var toD = filterModel.ToDate.Value;
                dapperExecution.SqlBuilder.Where("t1.CreatedDate < @toDate", new { toDate = toD.AddDays(1)});
            }
            if (filterModel.IsHot.HasValue && filterModel.IsHot.Value)
            {
                //dapperExecution.SqlBuilder.Where("t1.IsHot = @isHot", new { isHot = filterModel.IsHot.Value });
                dapperExecution.SqlBuilder.Where("t4.ASCII = @noibat", new { noibat = "khuyen-mai-noi-bat" });

            }else
                        if (!string.IsNullOrEmpty(filterModel.ArticleTypeCode))
            {
                dapperExecution.SqlBuilder.Where("t4.NameAscii = @nameAscii", new { nameAscii = filterModel.ArticleTypeCode });
            }

            if (filterModel.ArticleType.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t3.ArticleCategoryId = @articleCategoryId", new { articleCategoryId = filterModel.ArticleType.Value });
            }


            var articleModels = dapperExecution.ExecutePaginateQuery<ArticleDto, PictureViewModel>(
                    (article, avatar) =>
                    {
                        if (!cache.TryGetValue(article.Id, out var articleViewModel))
                        {
                            articleViewModel = article;
                            cache.Add(article.Id, articleViewModel);
                        }

                        articleViewModel.Avatar = avatar;
                        return articleViewModel;
                    }, "Id,Id");

            return articleModels;
        }

        public ArticleDto Find(int id)
        {
            return this.FindByDynamicId(id);
        }

        public ArticleDto Find(string id)
        {
            return this.FindByDynamicId(id);
        }

        private ArticleDto FindByDynamicId(object id)
        {
            var cache = new Dictionary<int, ArticleDto>();
            var dapperExecution = BuildByTemplate<ArticleDto>();

            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Name");
            dapperExecution.SqlBuilder.Select("t2.FileName");
            dapperExecution.SqlBuilder.Select("t2.FilePath");
            dapperExecution.SqlBuilder.Select("t2.Size");
            dapperExecution.SqlBuilder.Select("t2.Extension");
            dapperExecution.SqlBuilder.Select("t2.RedirectLink");

            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.ArticleCategoryId");

            dapperExecution.SqlBuilder.Where(id is int ? "t1.Id = @id" : "t1.IdentityGuid = @id", new { id });

            dapperExecution.SqlBuilder.LeftJoin("Pictures t2 ON t1.AvatarId = t2.Id AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "ArticleArticleCategories t3 ON t1.Id = t3.ArticleId AND t3.IsActive = TRUE AND t3.IsDeleted = FALSE");

            var articleModel = dapperExecution.ExecuteQuery<ArticleDto, PictureViewModel, ArticleArticleCategoryDto>(
                    (article, avatar, articleArticleCategory) =>
                    {
                        if (!cache.TryGetValue(article.Id, out var articleViewModel))
                        {
                            articleViewModel = article;
                            cache.Add(article.Id, articleViewModel);
                        }

                        if (articleArticleCategory != null && articleViewModel.ArticleCategories.All(e => e != articleArticleCategory.ArticleCategoryId))
                        {
                            articleViewModel.ArticleCategories.Add(articleArticleCategory.ArticleCategoryId);
                        }

                        articleViewModel.Avatar = avatar;
                        return articleViewModel;
                    }, "Id,Id,Id")
                .Distinct().FirstOrDefault();

            return articleModel;
        }
    }
}
