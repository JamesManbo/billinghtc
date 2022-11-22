using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticResource.API.Models
{
    public class TemporaryFileModel
    {
        public string FileName { get; set; }
        public string TemporaryUrl { get; set; }
        public string FullTemporaryUrl { get; set; }
    }
}
