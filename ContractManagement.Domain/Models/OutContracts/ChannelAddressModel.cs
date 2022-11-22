using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.OutContracts
{
    public class ChannelAddressModel
    {
        public string Cid { get; set; }
        public InstallationAddress StartPointAddress { get; set; }
        public InstallationAddress EndPointAddress { get; set; }
    }
}
