using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Notification;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notification.API.Models
{
    [BsonIgnoreExtraElements]
    public class Notification : BaseBsonEntity, ICloneable
    {
        public NotificationType Type { get; set; }
        public NotificationZone Zone { get; set; }
        public NotificationCategory Category { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
        public string Receiver { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverToken { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Payload { get; set; }
        public bool IsRead { get; set; }
        public bool IsStaff { get; set; }
        public string Platform { get; set; }
        public string ReadableTime => ToReadableTime(CreatedDate);

        public string ToReadableTime(DateTime value)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - value.Ticks);
            double delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return "Vài giây trước";
            }
            if (delta < 120)
            {
                return "Một phút trước";
            }
            if (delta < 3000) // 50 * 60
            {
                return ts.Minutes + " phút trước";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " giờ trước";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "Hôm qua lúc " + value.ToString("HH:mm");
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " ngày trước";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "Một tháng trước" : months + " tháng trước";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "Một năm trước" : years + " năm trước";
        }
        public object Clone()
        {
            var notificationClone = (Notification) MemberwiseClone();
            notificationClone.CreatedDate = DateTime.UtcNow;
            notificationClone.UpdatedDate = null;
            notificationClone.IsRead = false;
            notificationClone.Id = null;
            return notificationClone;
        }

    }
}
