using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.CommonModels
{
    public class PictureViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? DisplayOrder { get; set; }
        public int PictureType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string TemporaryUrl { get; set; }
        public bool IsUpdating { get; set; }

        public string OptimizedUrl { get; set; }
        public string W160Url { get; set; }
        public string W1280Url { get; set; }
        public string W320Url { get; set; }
        public string W640Url { get; set; }
        public string ThumbUrl { get; set; }
    }
}
