using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmIppools
    {
        public int Id { get; set; }
        public sbyte Type { get; set; }
        public string Name { get; set; }
        public string Fromip { get; set; }
        public string Toip { get; set; }
        public string Descr { get; set; }
        public int Nextpoolid { get; set; }
    }
}
