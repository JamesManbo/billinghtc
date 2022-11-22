using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class InstallationAddressDTO
    {
        public string Street { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public string FullAddress { get; set; }
    }
}
