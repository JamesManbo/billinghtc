using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.ResourceConfig;
using StaticResource.API.Helper;

namespace StaticResource.API.Models
{
    public class PictureModel
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? DisplayOrder { get; set; }
        public int PictureType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }

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
        public string TemporaryUrl { get; set; }
    }
}
