using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class Radpostauth
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Reply { get; set; }
        public DateTime Authdate { get; set; }
        public string Nasipaddress { get; set; }
    }
}
