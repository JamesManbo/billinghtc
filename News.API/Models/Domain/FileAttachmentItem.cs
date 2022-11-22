using News.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models.Domain
{
    public class FileAttachmentItem
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormattedCreatedDate => CreatedDate.ToString("dd/MM/yyyy hh:mm");
        public string Culture { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? Order { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string TemporaryUrl { get; set; }
        public string ArticleContentURL =>
            $"{StaticConfigs.MediaSourceURL}{StaticConfigs.FileArticleContentFolder}{FilePath}".ResolveUrl();
    }
}
