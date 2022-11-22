using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class ContractorFilterModel : RequestFilterModel
    {
        public string ProjectIds { get; set; }
        public string ServiceIds { get; set; }
    }
}
