using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class ContractFilterModel : RequestFilterModel
    {
        public string ProjectIds { get; set; }
        public string ServiceIds { get; set; }
        public int? ContractorId { get; set; }
        public int? ContractStatusId { get; set; }
    }
}
