using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    [Table("ApplicationUserGroups")]
    public class ApplicationUserGroup : Entity
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
    }
}
