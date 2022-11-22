using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmOnlinecm
    {
        public string Username { get; set; }
        public string Maccm { get; set; }
        public sbyte? Enableuser { get; set; }
        public string Staticipcm { get; set; }
        public string Maccpe { get; set; }
        public string Ipcpe { get; set; }
        public sbyte? Ipmodecpe { get; set; }
        public int? Cmtsid { get; set; }
        public int? Groupid { get; set; }
        public string Groupname { get; set; }
        public decimal? Snrds { get; set; }
        public decimal? Snrus { get; set; }
        public decimal? Txpwr { get; set; }
        public decimal? Rxpwr { get; set; }
        public decimal? Pingtime { get; set; }
        public string Upstreamname { get; set; }
        public int? Ifidx { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
