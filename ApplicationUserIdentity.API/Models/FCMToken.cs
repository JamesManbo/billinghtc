using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    [Table("FCMTokens")]
    public class FCMToken : Entity
    {
        public string ReceiverId { get; set; }
        public string Token { get; set; }
        public string Platform { get; set; }
    }
}
