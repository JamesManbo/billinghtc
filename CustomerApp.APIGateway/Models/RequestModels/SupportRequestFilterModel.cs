using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.RequestModels
{
    public class SupportRequestFilterModel: RequestFilterModel
    {
        public string CustomerId { get; set; }
        public string CreatedBy { get; set; }
    }
}
