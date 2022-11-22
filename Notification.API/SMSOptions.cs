using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API
{
    public class SMSOptions
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string ServiceID { get; set; }
        public int MtID { get; set; }
        public string CpID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
