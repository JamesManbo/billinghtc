using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class SettingAccountCommand
    {
        public string IdentityGuid { get; set; }
        public string FullName { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Email { get; set; }
        public bool IsAllowEmail { get; set; }
        public bool IsAllowSMS { get; set; }
    }
}
