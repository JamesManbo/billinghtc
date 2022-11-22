using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.AuthModels
{
    public class ChangePasswordResultDTO
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
