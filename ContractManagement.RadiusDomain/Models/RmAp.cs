using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmAp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte Enable { get; set; }
        public sbyte Accessmode { get; set; }
        public string Ip { get; set; }
        public string Community { get; set; }
        public string Apiusername { get; set; }
        public string Apipassword { get; set; }
        public string Description { get; set; }
    }
}
