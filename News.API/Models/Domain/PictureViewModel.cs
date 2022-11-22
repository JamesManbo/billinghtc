using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Extensions;
using Global.Configs.ResourceConfig;

namespace News.API.Models.Domain
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

        public string OptimizedUrl =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageOptimizeFolder}{FilePath}".ResolveUrl();
        public string W160Url =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageW160Folder}{FilePath}".ResolveUrl();
        public string W1280Url =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageW1280Folder}{FilePath}".ResolveUrl();
        public string W320Url =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageW320Folder}{FilePath}".ResolveUrl();
        public string W640Url =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageW640Folder}{FilePath}".ResolveUrl();
        public string ThumbUrl =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageThumbFolder}{FilePath}".ResolveUrl();
        public string ArticleContentURL =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.ImageArticleContentFolder}{FilePath}".ResolveUrl();
    }
}
