using GenericRepository;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleRepository
{
    public interface IArticleRepository : ICrudRepository<Article, int>
    {
        bool CheckExitLink(string link, int id);
    }
}
