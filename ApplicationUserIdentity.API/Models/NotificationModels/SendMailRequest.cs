using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.NotificationModels
{
    public class SendMailRequest
    {
        public string Emails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
