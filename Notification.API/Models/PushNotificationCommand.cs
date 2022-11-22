using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class PushNotificationCommand
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string TokenDevice { get; set; } 
        public string Data { get; set; }
    }
}
