using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Models
{
    public class CreateDraftLocationCommand
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentId { get; set; }
    }
}
