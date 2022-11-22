using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string MobilePhoneNo { get; set; }
        public int? AvatarId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; } //Số CMT
    }
}
