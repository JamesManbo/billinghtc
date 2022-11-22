using GenericRepository;
using GenericRepository.Configurations;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleCategoryRepository
{
    public class ArticleCategoryRepository : CrudRepository<ArticleCategory, int>, IArticleCategoryRepository
    {
        private readonly NewsDbContext _newsDbContext;
        public ArticleCategoryRepository(NewsDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _newsDbContext = context;
        }

        public bool CheckExitName(string name, int id)
        {
            var lstNameArticleCategory = _newsDbContext.ArticleCategories.Where(x => x.Name == name.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNameArticleCategory == 0) // không tồn tại tên 
            {
                return true;
            }
            else if (id > 0 && lstNameArticleCategory == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckExitNameAscii(string nameAscii, int id)
        {
            var lstNameAscii = _newsDbContext.ArticleCategories.Where(x => x.NameAscii == nameAscii.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstNameAscii == 0) // không tồn tại name Ascii
            {
                return true;
            }
            else if (id > 0 && lstNameAscii == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
