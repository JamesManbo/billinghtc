using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    public class UserRequestFilterModel: RequestFilterModel
    {
        public int? GroupId { get; set; }
        public string IdentityGuids { get; set; }
        public string MarketAreaIds { get; set; }
        public string ProjectIds { get; set; }

        public int? IndustryId { get; set; }
    }
}
