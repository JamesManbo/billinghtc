using GenericRepository;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleArticleCategoryRepository
{
    public interface IArticleArticleCategoryRepository : ICrudRepository<ArticleArticleCategory, int>
    {
        Task DeleteAllMapArticleArticleCategoryByArticleId(int categoryId);
    }
}
