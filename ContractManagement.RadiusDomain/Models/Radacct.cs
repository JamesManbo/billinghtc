using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class Radacct
    {
        public long Radacctid { get; set; }
        public string Acctsessionid { get; set; }
        public string Acctuniqueid { get; set; }
        public string Username { get; set; }
        public string Groupname { get; set; }
        public string Realm { get; set; }
        public string Nasipaddress { get; set; }
        public string Nasportid { get; set; }
        public string Nasporttype { get; set; }
        public DateTime? Acctstarttime { get; set; }
        public DateTime? Acctstoptime { get; set; }
        public int? Acctsessiontime { get; set; }
        public string Acctauthentic { get; set; }
        public string ConnectinfoStart { get; set; }
        public string ConnectinfoStop { get; set; }
        public long? Acctinputoctets { get; set; }
        public long? Acctoutputoctets { get; set; }
        public string Calledstationid { get; set; }
        public string Callingstationid { get; set; }
        public string Acctterminatecause { get; set; }
        public string Servicetype { get; set; }
        public string Framedprotocol { get; set; }
        public string Framedipaddress { get; set; }
        public int? Acctstartdelay { get; set; }
        public int? Acctstopdelay { get; set; }
        public string Xascendsessionsvrkey { get; set; }
        public DateTime? Accttime { get; set; }
        public int? Srvid { get; set; }
        public sbyte? Dailynextsrvactive { get; set; }
        public int? Apid { get; set; }
    }
}
