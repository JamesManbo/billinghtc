using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmCards
    {
        public long Id { get; set; }
        public string Cardnum { get; set; }
        public string Password { get; set; }
        public decimal Value { get; set; }
        public DateTime Expiration { get; set; }
        public string Series { get; set; }
        public DateTime Date { get; set; }
        public string Owner { get; set; }
        public DateTime Used { get; set; }
        public sbyte Cardtype { get; set; }
        public sbyte Revoked { get; set; }
        public long Downlimit { get; set; }
        public long Uplimit { get; set; }
        public long Comblimit { get; set; }
        public long Uptimelimit { get; set; }
        public int Srvid { get; set; }
        public string Transid { get; set; }
        public sbyte Active { get; set; }
        public long Expiretime { get; set; }
        public sbyte Timebaseexp { get; set; }
        public sbyte Timebaseonline { get; set; }
    }
}
