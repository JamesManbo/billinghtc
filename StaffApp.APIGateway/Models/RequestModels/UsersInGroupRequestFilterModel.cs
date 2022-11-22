using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class UsersInGroupRequestFilterModel : RequestFilterModel
    {
        public int? GroupId { get; set; }
    }
}
