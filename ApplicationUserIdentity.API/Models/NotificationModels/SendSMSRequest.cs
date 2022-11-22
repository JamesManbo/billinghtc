using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.NotificationModels
{
    public class SendSMSRequest
    {
        public string PhoneNumbers { get; set; }
        public string Message { get; set; }
    }
}
