using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmWlan
    {
        public string Maccpe { get; set; }
        public short? Signal { get; set; }
        public short? Ccq { get; set; }
        public short? Snr { get; set; }
        public string Apip { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
