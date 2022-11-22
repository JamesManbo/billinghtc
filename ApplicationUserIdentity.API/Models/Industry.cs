using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models
{
    [Table("Industries")]
    public class Industry : Entity
    {
        [StringLength(512)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
