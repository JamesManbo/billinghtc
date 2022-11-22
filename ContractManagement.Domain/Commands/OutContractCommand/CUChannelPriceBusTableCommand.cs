using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUChannelPriceBusTableCommand
        : DeploymentChannelPriceBusTableCommand
    {
        public int? ChannelId { get; set; }
    }
}
