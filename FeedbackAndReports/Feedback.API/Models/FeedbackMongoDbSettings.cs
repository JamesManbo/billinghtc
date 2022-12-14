using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedback.API.Models
{
    public class FeedbackMongoDbSettings
    {
        public MongoServerAddressInfo[] Servers { get; set; }
    }

    public class MongoServerAddressInfo
    {
        public string Host { get; set; }
        public int Port { get; set; } = 27017;
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
