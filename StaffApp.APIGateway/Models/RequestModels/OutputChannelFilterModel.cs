using Global.Models.Filter;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class OutputChannelFilterModel : RequestFilterModel
    {
        public OutputChannelPointTypeEnum? PointType { get; set; }
    }
}
