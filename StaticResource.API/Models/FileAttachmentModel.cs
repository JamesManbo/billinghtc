using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.ResourceConfig;
using StaticResource.API.Helper;

namespace StaticResource.API.Models
{
    public class FileAttachmentModel
    {
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? Order { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string TemporaryUrl { get; set; }
    }
}
