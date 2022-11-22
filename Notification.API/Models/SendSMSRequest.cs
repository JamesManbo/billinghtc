using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class SendSMSRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
