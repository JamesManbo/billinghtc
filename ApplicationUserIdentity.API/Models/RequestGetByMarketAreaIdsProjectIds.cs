using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    public class RequestGetByMarketAreaIdsProjectIds
    {
        public string MarketAreaIds { get; set; }
        public string ProjectIds { get; set; }
    }
}
