using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.CommonModels;

namespace StaffApp.APIGateway.Models.HelpCenterModels
{
    public class HelpCenterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
