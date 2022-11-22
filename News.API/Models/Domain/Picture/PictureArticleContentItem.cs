using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models.Domain.Picture
{
    public class PictureArticleContentItem
    {
        public string TemporaryUrl { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PictureType { get; set; }
    }
}
