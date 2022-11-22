using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.NotificationModels
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public NotificationType Type { get; set; }
        public NotificationZone Zone { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
        public string Receiver { get; set; }
        public string ReceiverId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Payload { get; set; }
        public bool IsRead { get; set; }
        public bool IsStaff { get; set; }
        public string ReadableTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormat { get { return CreatedDate.ToString("dd/MM/yyyy"); } }

    }

    public enum NotificationType : int
    {
        SystemAlert = 0,
        BillingAlert,
        ContractExpirationAlert,
        Private,
    }

    public enum NotificationZone
    {
        Contract = 0,
        Billing,
        FeedBack,
        Organization
    }
}
