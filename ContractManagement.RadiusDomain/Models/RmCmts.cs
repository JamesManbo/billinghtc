using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmCmts
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Community { get; set; }
        public string Descr { get; set; }
    }
}
