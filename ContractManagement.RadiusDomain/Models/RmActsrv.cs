using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmActsrv
    {
        [Key]
        public long Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Username { get; set; }
        public int Srvid { get; set; }
        public sbyte Dailynextsrvactive { get; set; }
    }
}
