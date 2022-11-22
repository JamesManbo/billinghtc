using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class PromotionFilterModel : RequestFilterModel
    {
        public bool IsHot { get; set; }
    }
}
