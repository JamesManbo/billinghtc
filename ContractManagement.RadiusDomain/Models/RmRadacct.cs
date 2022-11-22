using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmRadacct
    {
        public long Radacctid { get; set; }
        public string Acctuniqueid { get; set; }
        public string Username { get; set; }
        public DateTime Acctstarttime { get; set; }
        public DateTime Acctstoptime { get; set; }
        public int Acctsessiontime { get; set; }
        public decimal Acctsessiontimeratio { get; set; }
        public long Dlbytesstart { get; set; }
        public long Dlbytesstop { get; set; }
        public long Dlbytes { get; set; }
        public decimal Dlratio { get; set; }
        public long Ulbytesstart { get; set; }
        public long Ulbytesstop { get; set; }
        public long Ulbytes { get; set; }
        public decimal Ulratio { get; set; }
    }
}
