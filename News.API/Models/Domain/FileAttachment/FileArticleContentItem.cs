using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models.Domain.FileAttachment
{
    public class FileArticleContentItem
    {
        public string TemporaryUrl { get; set; }
        public int FileType { get; set; }
    }
}
