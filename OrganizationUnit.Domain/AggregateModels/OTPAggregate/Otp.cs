using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.OTPAggregate
{
    [Table("Otps")]
    public class OtpEntity : Entity, IAggregateRoot
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
        public DateTime? DateExpired { get; set; }
        public bool? IsUse { get; set; }
    }
}
