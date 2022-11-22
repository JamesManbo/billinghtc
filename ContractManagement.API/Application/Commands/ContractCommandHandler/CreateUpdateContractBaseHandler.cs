using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;

namespace ContractManagement.API.Application.Commands.ContractCommandHandler
{
    public abstract class CreateUpdateContractBaseHandler
    {
        public void MapPackageCommand(ref CUOutContractChannelCommand packageCommand,
            ServicePackageDTO packageInfo)
        {
            packageCommand.ServiceId = packageInfo.ServiceId;
            packageCommand.ServicePackageId = packageInfo.Id;
            packageCommand.ServiceName = packageInfo.ServiceName;
            packageCommand.PackageName = packageInfo.PackageName;
            packageCommand.OrgPackagePrice = packageInfo.Price;
            packageCommand.BandwidthLabel = packageInfo.BandwidthLabel;
            packageCommand.InternationalBandwidth = packageInfo.InternationalBandwidth;
            packageCommand.DomesticBandwidth = packageInfo.DomesticBandwidth;
            packageCommand.InternationalBandwidthUom = packageInfo.InternationalBandwidthUom;
            packageCommand.DomesticBandwidth = packageInfo.DomesticBandwidth;
            packageCommand.DomesticBandwidthUom = packageInfo.DomesticBandwidthUom;
        }        
    }
}
