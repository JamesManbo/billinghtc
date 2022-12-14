using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.User
{
    public class UserContactInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
    }
}
