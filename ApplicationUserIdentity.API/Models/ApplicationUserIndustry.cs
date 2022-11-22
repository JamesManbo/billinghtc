using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    [Table("ApplicationUserIndustries")]
    public class ApplicationUserIndustry: Entity
    {
        public int UserId { get; set; }
        public int IndustryId { get; set; }
    }
}
