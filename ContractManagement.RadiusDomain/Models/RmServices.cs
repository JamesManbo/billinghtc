using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmServices
    {
        public int Srvid { get; set; }
        public string Srvname { get; set; }
        public string Descr { get; set; }
        public int Downrate { get; set; }
        public int Uprate { get; set; }
        public sbyte Limitdl { get; set; }
        public sbyte Limitul { get; set; }
        public sbyte Limitcomb { get; set; }
        public sbyte Limitexpiration { get; set; }
        public sbyte Limituptime { get; set; }
        public string Poolname { get; set; }
        public decimal Unitprice { get; set; }
        public decimal Unitpriceadd { get; set; }
        public sbyte Timebaseexp { get; set; }
        public sbyte Timebaseonline { get; set; }
        public int Timeunitexp { get; set; }
        public int Timeunitonline { get; set; }
        public int Trafficunitdl { get; set; }
        public int Trafficunitul { get; set; }
        public int Trafficunitcomb { get; set; }
        public int Inittimeexp { get; set; }
        public int Inittimeonline { get; set; }
        public int Initdl { get; set; }
        public int Initul { get; set; }
        public int Inittotal { get; set; }
        public sbyte Srvtype { get; set; }
        public sbyte Timeaddmodeexp { get; set; }
        public sbyte Timeaddmodeonline { get; set; }
        public sbyte Trafficaddmode { get; set; }
        public sbyte Monthly { get; set; }
        public sbyte Enaddcredits { get; set; }
        public int Minamount { get; set; }
        public int Minamountadd { get; set; }
        public sbyte Resetcounters { get; set; }
        public sbyte Pricecalcdownload { get; set; }
        public sbyte Pricecalcupload { get; set; }
        public sbyte Pricecalcuptime { get; set; }
        public decimal Unitpricetax { get; set; }
        public decimal Unitpriceaddtax { get; set; }
        public sbyte Enableburst { get; set; }
        public int Dlburstlimit { get; set; }
        public int Ulburstlimit { get; set; }
        public int Dlburstthreshold { get; set; }
        public int Ulburstthreshold { get; set; }
        public int Dlbursttime { get; set; }
        public int Ulbursttime { get; set; }
        public int Enableservice { get; set; }
        public long Dlquota { get; set; }
        public long Ulquota { get; set; }
        public long Combquota { get; set; }
        public long Timequota { get; set; }
        public short Priority { get; set; }
        public int Nextsrvid { get; set; }
        public int Dailynextsrvid { get; set; }
        public int Disnextsrvid { get; set; }
        public sbyte Availucp { get; set; }
        public sbyte Renew { get; set; }
        public sbyte Carryover { get; set; }
        public string Policymapdl { get; set; }
        public string Policymapul { get; set; }
        public string Custattr { get; set; }
        public sbyte Gentftp { get; set; }
        public string Cmcfg { get; set; }
        public sbyte Advcmcfg { get; set; }
        public int Addamount { get; set; }
        public sbyte Ignstatip { get; set; }
    }
}
