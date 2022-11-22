using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.Otp
{
    [Table("Otps")]
    public class OtpEntity : Entity
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool? IsUse { get; set; }
    }
}
