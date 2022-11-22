using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class NotificationFilterModel
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int? NotificationType { get; set; }
        public string ReceiverId { get; set; }
        public bool? IsRead { get; set; }
    }
}
