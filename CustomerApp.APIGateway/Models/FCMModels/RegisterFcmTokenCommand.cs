using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.FCMModels
{
    public class RegisterFcmTokenCommand
    {
        public string ReceiverId { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
    }
}
