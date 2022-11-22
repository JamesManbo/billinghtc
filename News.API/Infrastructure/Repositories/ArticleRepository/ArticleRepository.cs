using GenericRepository;
using GenericRepository.Configurations;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleRepository
{
    public class ArticleRepository : CrudRepository<Article, int>, IArticleRepository
    {
        private readonly NewsDbContext _newsDbContext;
        public ArticleRepository(NewsDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _newsDbContext = context;
        }

        public bool CheckExitLink(string link, int id)
        {
            var lstLinkArticle = _newsDbContext.Articles.Where(x => x.Link == link.Trim() && x.IsDeleted == false && x.Id != id).Count();
            if (id == 0 && lstLinkArticle == 0) // không tồn tại link 
            {
                return true;
            }
            else if (id > 0 && lstLinkArticle == 0)
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
