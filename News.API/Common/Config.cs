using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Common
{
    public static class StaticConfigs
    {
        public static string MediaSourceURL { get; set; }
        public static string ImageOptimizeFolder { get; set; }
        public static string ImageW1280Folder { get; set; }
        public static string ImageW640Folder { get; set; }
        public static string ImageW320Folder { get; set; }
        public static string ImageW160Folder { get; set; }
        public static string ImageThumbFolder { get; set; }
        public static string ImageArticleContentFolder { get; set; }
        public static string FileArticleContentFolder { get; set; }
    }
}
