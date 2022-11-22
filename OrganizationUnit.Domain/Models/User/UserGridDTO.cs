using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Domain.Models.User
{
    public class UserGridDTO
    {
        public UserGridDTO()
        {
            OrganizationUnitIds = new List<int> { };
        }

        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string Label { get; set; }
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
        public bool IsLock { get; set; }
        public bool IsEnterprise { get; set; }
        public bool? IsLeader { get; set; }
        public bool IsActive { get; set; }
        public bool IsPartner { get; set; }
        public List<int> OrganizationUnitIds { get; set; }
        public string OrganizationUnitNames { get; set; }
    }
}
