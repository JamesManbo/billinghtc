using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.RequestModels
{
    public class ChangePasswordByUserNameRequest
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
    }
}
