using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    [Table("ApplicationUserUserGroups")]
    public class ApplicationUserUserGroup : Entity
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
