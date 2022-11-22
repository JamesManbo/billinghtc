using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class AddressDTO
    {
        public string Street { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
    }
}
