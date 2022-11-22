using Global.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Notification
{
    public class PushNotificationRequest
    {
        public NotificationType Type { get; set; }
        public NotificationZone Zone { get; set; }
        public NotificationCategory Category { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
        public string Receiver { get; set; }
        public string ReceiverId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Payload { get; set; }
    }
}
