using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class NotificationFilterModel: RequestFilterModel
    {
        public int? NotificationType { get; set; }
        public string ReceiverId { get; set; }
        public bool? IsRead { get; set; }
    }
}
