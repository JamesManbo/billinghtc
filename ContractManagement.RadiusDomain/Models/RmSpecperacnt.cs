using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmSpecperacnt
    {
        public int Id { get; set; }
        public int Srvid { get; set; }
        public TimeSpan Starttime { get; set; }
        public TimeSpan Endtime { get; set; }
        public decimal Timeratio { get; set; }
        public decimal Dlratio { get; set; }
        public decimal Ulratio { get; set; }
        public sbyte Connallowed { get; set; }
        public sbyte Mon { get; set; }
        public sbyte Tue { get; set; }
        public sbyte Wed { get; set; }
        public sbyte Thu { get; set; }
        public sbyte Fri { get; set; }
        public sbyte Sat { get; set; }
        public sbyte Sun { get; set; }
    }
}
