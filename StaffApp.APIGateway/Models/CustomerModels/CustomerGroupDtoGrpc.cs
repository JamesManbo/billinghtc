using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CustomerModels
{
    public class CustomerGroupDtoGrpc
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
    }
}
