using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.NotificationModels
{
    public class SendNotificationFilterRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public UserRequestFilterModel FilterSelect { get; set; }
        public List<int> UnSelectIds { get; set; }
        public bool IsSendNotification { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsSendSMS { get; set; }
    }
}
