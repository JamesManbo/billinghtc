using GenericRepository;
using News.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.ArticleCategoryRepository
{
    public interface IArticleCategoryRepository : ICrudRepository<ArticleCategory, int>
    {
        bool CheckExitName(string name, int id);
        bool CheckExitNameAscii(string nameAscii, int id);
    }
}
