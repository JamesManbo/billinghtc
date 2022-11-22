using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmChangesrv
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Newsrvid { get; set; }
        public string Newsrvname { get; set; }
        public DateTime Scheduledate { get; set; }
        public DateTime Requestdate { get; set; }
        public sbyte Status { get; set; }
        public string Transid { get; set; }
        public string Requested { get; set; }
    }
}
