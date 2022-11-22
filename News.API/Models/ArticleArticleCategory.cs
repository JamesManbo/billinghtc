using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models
{
    [Table("ArticleArticleCategories")]
    public class ArticleArticleCategory : Entity
    {
        public int ArticleId { get; set; }
        public int ArticleCategoryId { get; set; }
    }
}
