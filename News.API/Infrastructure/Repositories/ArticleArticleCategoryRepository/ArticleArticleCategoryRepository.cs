using GenericRepository;
using GenericRepository.Configurations;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleArticleCategoryRepository
{
    public class ArticleArticleCategoryRepository : CrudRepository<ArticleArticleCategory, int>, IArticleArticleCategoryRepository
    {
        NewsDbContext _context;
        public ArticleArticleCategoryRepository(NewsDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public async Task DeleteAllMapArticleArticleCategoryByArticleId(int categoryId)
        {
            var lstExist = _context.ArticleArticleCategories.Where(m => m.ArticleId == categoryId && m.IsDeleted == false && m.IsActive == true).ToList();
            if (lstExist != null && lstExist.Count > 0)
            {
                for (int i = 0; i < lstExist.Count; i++)
                {
                    var ob = lstExist.ElementAt(i);
                    ob.IsDeleted = true;
                    _context.Update(ob);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
