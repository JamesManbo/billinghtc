using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.FCM
{
    public class FCMTokenDto
    {
        public int Id { get; set; }
        public string ReceiverId { get; set; }
        public string Receiver { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
    }
}
