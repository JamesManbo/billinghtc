using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models.Domain
{
    public class ArticleArticleCategoryDto
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int ArticleCategoryId { get; set; }
    }
}
