using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class Radippool
    {
        public uint Id { get; set; }
        public string PoolName { get; set; }
        public string Framedipaddress { get; set; }
        public string Nasipaddress { get; set; }
        public string Calledstationid { get; set; }
        public string Callingstationid { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public string Username { get; set; }
        public string PoolKey { get; set; }
    }
}
