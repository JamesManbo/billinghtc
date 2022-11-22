using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Notification
{
    public class SendMailRequest
    {
        public string Emails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
