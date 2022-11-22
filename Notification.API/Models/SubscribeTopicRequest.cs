using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class SubscribeTopicRequest
    {
        public string Token { get; set; }
        public string Topics { get; set; }
    }
}
