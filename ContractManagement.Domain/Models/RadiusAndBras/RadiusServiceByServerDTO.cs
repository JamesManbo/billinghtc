using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.RadiusAndBras
{
    public class RadiusServiceByServerDTO
    {
        public int RadiusServerId { get; set; }
        public string RadiusServerName { get; set; }
        public string RadiusServerIpAddress { get; set; }
        public IEnumerable<RadiusServiceDTO> RadiusServices { get; set; }
    }
}
