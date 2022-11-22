using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.Feedback
{
    public class RequestSupportFilterModel : RequestFilterModel
    {
        public string Source { get; set; }

        public string CreatedBy { get; set; }
    }
}
