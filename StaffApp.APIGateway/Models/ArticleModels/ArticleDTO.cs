using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ArticleModels
{
    public class ArticleDTO
    {
        public ArticleDTO()
        {
            ArticleCategories = new List<int>();
        }
        public int Id { get; set; }
        public Guid IdentityGuid { get; set; }
        public string Title { get; set; } //tiêu đề bài viết
        public string Name { get; set; } //tên bài viết bài viết
        public int? AvatarId { get; set; }
        public PictureViewDTO Avatar { get; set; }
        public string Content { get; set; } // nội dung bài viết
        public List<int> ArticleCategories { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string RawDescription { get; set; }
    }
}
