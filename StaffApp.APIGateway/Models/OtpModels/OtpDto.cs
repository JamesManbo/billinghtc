using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.OtpModels
{
    public class OtpDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Otp { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DateExpired { get; set; }
        public bool? IsUse { get; set; }
    }
}
