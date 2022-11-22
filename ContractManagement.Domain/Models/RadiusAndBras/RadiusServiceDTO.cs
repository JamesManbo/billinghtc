using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.RadiusAndBras
{
    public class RadiusServiceDTO
    {
        public int Srvid { get; set; }
        public string Srvname { get; set; }
        public string Descr { get; set; }
        public int Downrate { get; set; }
        public int Uprate { get; set; }
    }
}
