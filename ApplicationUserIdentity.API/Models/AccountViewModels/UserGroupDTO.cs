using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    public class UserGroupDTO
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
