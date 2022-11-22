using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.ConfigurationSettingUser
{
    public class ConfigurationSettingUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool AllowSendEmail { get; set; }
        public bool AllowSendNotification { get; set; }
        public bool AllowSendSMS { get; set; }
    }
}
