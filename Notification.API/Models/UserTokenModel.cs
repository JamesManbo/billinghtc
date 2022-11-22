using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class UserTokenModel
    {
        public string Token { get; set; }
        public string ReceiverId { get; set; }
        public string Receiver { get; set; }
        public string Platform { get; set; }
    }
}
