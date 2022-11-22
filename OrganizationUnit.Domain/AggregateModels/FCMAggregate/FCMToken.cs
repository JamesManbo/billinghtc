using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.FCMAggregate
{
    [Table("FCMTokens")]
    public class FCMToken : Entity
    {
        public string ReceiverId { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
    }
}
