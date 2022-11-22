using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmIas
    {
        public int Iasid { get; set; }
        public string Iasname { get; set; }
        public decimal Price { get; set; }
        public long Downlimit { get; set; }
        public long Uplimit { get; set; }
        public long Comblimit { get; set; }
        public long Uptimelimit { get; set; }
        public long Expiretime { get; set; }
        public sbyte Timebaseonline { get; set; }
        public sbyte Timebaseexp { get; set; }
        public int Srvid { get; set; }
        public sbyte Enableias { get; set; }
        public sbyte Expiremode { get; set; }
        public DateTime Expiration { get; set; }
        public int Simuse { get; set; }
    }
}
