using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Configs.ResourceConfig
{
    public static class ResourceGlobalConfigs
    {
        public static string MediaSourceURL = "https://bil-cms-api.htc-itc.vn/media/";
        public static string AllowedImageFileFormat = ".jpg,.png,.jpeg,.bmp";
        public static string ResourceTempFolder = "materials/temp/";
        public static string ImageOriginalFolder = "materials/images/originals/";
        public static string ImageOptimizeFolder = "materials/images/optimizeds/";
        public static string ImageW1280Folder = "materials/images/w1280s/";
        public static string ImageW640Folder = "materials/images/w640s/";
        public static string ImageW320Folder = "materials/images/w320s/";
        public static string ImageW160Folder = "materials/images/w160s/";
        public static string ImageThumbFolder = "materials/images/thumbnails/";
        public static string FileFolder = "materials/files/";
        public static string NumberOfAttemptFindingTemporaryFile = "5";
        public static string ImageArticleContentFolder { get; set; }
    }
}
