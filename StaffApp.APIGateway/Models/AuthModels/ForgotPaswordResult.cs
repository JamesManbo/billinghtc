using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class ForgotPaswordResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public DateTime? DateExpired { get; set; }
    }
}
