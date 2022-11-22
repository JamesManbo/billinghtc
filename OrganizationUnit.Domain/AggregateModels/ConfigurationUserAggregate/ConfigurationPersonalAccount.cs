using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate
{
    [Table("ConfigurationPersonalAccounts")]
    public class ConfigurationPersonalAccount : Entity
    {
        public int UserId { get; set; }
        public bool AllowSendEmail { get; set; }
        public bool AllowSendNotification { get; set; }
        public bool AllowSendSMS { get; set; }
        //public int ChangeRecordExport { get; set; }

        public ConfigurationPersonalAccount()
        {

        }

        public ConfigurationPersonalAccount(int userId, bool allowSendEmail, bool allowSendNotification, bool allowSendSMS)
        {
            UserId = userId;
            AllowSendEmail = allowSendEmail;
            AllowSendNotification = allowSendNotification;
            AllowSendSMS = allowSendSMS;
            DisplayOrder = 1;
        }
    }
}
