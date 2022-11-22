using GenericRepository.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models
{
    [Table("Articles")]
    public class Article : Entity
    {
        public Guid IdentityGuid { get; set; }
        [StringLength(256)]
        public string Title { get; set; } //tiêu đề bài viết
        [StringLength(256)]
        public string TitleAscii { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public int? AvatarId { get; set; }

        [Column(TypeName = "TEXT")]
        public string Content { get; set; } // nội dung chi tiết bài viết

        [Column(TypeName = "NVARCHAR(5000)")]
        public string Description { get; set; } // nội dung tóm tắt bài viết
        [Column(TypeName = "NVARCHAR(500)")]
        public string Link { get; set; } // Link bài viết

        [Column(TypeName = "NVARCHAR(5000)")]
        public string RawDescription { get; set; } // nội dung tóm tắt bài viết
        public bool IsHot { get; set; }
    }
}
