using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.AuthModels
{
    public class LoginResultDTO
    {
        public string Token { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
