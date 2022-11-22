using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmPhpsess
    {
        public string Managername { get; set; }
        public string Ip { get; set; }
        public string Sessid { get; set; }
        public DateTime Lastact { get; set; }
        public sbyte Closed { get; set; }
    }
}
