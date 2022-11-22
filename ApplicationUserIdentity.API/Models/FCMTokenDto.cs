using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    public class FCMTokenDto
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; }
        public string Receiver { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
    }
}
