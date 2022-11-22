using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class ForgotPasswordRequest
    {
        public string UserName { get; set; }
        public string Otp { get; set; }
    }
}
