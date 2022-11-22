using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmSpecperbw
    {
        public int Id { get; set; }
        public int Srvid { get; set; }
        public TimeSpan Starttime { get; set; }
        public TimeSpan Endtime { get; set; }
        public int Dlrate { get; set; }
        public int Ulrate { get; set; }
        public int Dlburstlimit { get; set; }
        public int Ulburstlimit { get; set; }
        public int Dlburstthreshold { get; set; }
        public int Ulburstthreshold { get; set; }
        public int Dlbursttime { get; set; }
        public int Ulbursttime { get; set; }
        public sbyte Enableburst { get; set; }
        public int Priority { get; set; }
        public sbyte Mon { get; set; }
        public sbyte Tue { get; set; }
        public sbyte Wed { get; set; }
        public sbyte Thu { get; set; }
        public sbyte Fri { get; set; }
        public sbyte Sat { get; set; }
        public sbyte Sun { get; set; }
    }
}
