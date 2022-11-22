using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.Authentication
{
    public class CustomerIdentityError
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
