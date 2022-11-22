using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.UserModels
{
    public class AccountCommand
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ProvinceIdentityGuid { get; set; }
        public string DistrictIdentityGuid { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
