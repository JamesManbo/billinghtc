﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels
{
    public class InstallationAddress
    {
        public string Street { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNumber { get; set; }
        public string FullAddress { get; set; }
    }
}
