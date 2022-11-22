using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Model
{
    public class LocationViewModel
    {
        public string Description { get; set; }
        public bool ShouldCommit { get; set; } = true;
    }
}
