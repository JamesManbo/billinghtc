using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.RequestModels
{
    public class ContractFilterModel : RequestFilterModel
    {
        public int ContractorId { get; set; }
    }
}
