using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models.Domain
{
    public class ArticleCategoryDto
    {
        public int Id { get; set; }
        public string ASCII { get; set; }

        public string Name { get; set; } //tên danh mục

        public string Description { get; set; } // diễn giải
        public int? ParentId { get; set; }
        public string TreePath { get; set; }
    }
}
