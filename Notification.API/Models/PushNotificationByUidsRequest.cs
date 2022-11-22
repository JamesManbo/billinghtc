using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class PushNotificationByUidsRequest
    {
        public string Uids { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
