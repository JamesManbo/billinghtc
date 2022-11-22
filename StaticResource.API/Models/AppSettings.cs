using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticResource.API.Models
{
    public class AppSettings
    {
        public string MediaSourceURL { get; set; }
        public string AllowedImageFileFormat { get; set; }
        public string NumberOfAttemptFindingTemporaryFile { get; set; }
    }
}
