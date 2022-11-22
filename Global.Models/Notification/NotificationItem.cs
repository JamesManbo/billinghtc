using System;

namespace Global.Models.Notification
{
    public class NotificationItem
    {
        public string Id { get; set; }
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
        public bool IsRead { get; set; }
        public bool IsShow { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum NotificationType : int
    {
        SystemAlert = 0,
        BillingAlert,
        Private,
        App
    }

    public enum NotificationZone
    {
        Contract = 0,
        Billing,
        FeedBack,
        Organization,
        Notification,
        ApplicationUser,
        Debt
    }

    public enum NotificationCategory
    {
        None = 0,
        ContractTransaction,
        Problem,
        News,
        EnterpriseContractExpiration,
        IndividualContractExpiration,
        ReceiptVoucher,
        ChannelRechargeExpiration,
    }
}
