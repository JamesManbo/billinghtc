using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models
{
    [Table("ArticleCategories")]
    public class ArticleCategory: Entity
    {
        [StringLength(256)]
        public string ASCII { get; set; }

        public string Name { get; set; } //tên danh mục

        [StringLength(256)]
        public string NameAscii { get; set; } // tên mã ASCII
        [StringLength(4000)]
        public string Description { get; set; } // diễn giải
        public int? ParentId { get; set; }
        [StringLength(4000)]
        public string TreePath { get; set; }
    }
}
