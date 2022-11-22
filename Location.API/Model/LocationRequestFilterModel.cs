using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Location
{
    public class LocationRequestFilterModel : RequestFilterModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public int? MinLevel { get; set; }
        public int? MaxLevel { get; set; }
        public string ParentId { get; set; }
    }
}
