using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmSyslog
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public int Eventid { get; set; }
        public string Data1 { get; set; }
    }
}
