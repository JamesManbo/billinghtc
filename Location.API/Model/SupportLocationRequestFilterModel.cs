using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Location
{
    public class SupportLocationRequestFilterModel : RequestFilterModel
    {
        public string IdentityGuid { get; set; }
    }
}
