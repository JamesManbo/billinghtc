using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AuthModels
{
    public class ConfigurationUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool AllowSendEmail { get; set; }
        public bool AllowSendNotification { get; set; }
        public bool AllowSendSMS { get; set; }
    }
}
