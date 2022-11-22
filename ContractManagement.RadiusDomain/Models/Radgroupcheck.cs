using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class Radgroupcheck
    {
        public uint Id { get; set; }
        public string Groupname { get; set; }
        public string Attribute { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
    }
}
