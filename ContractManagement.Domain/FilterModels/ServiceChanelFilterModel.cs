using ContractManagement.Domain.AggregatesModel.ServicePackages;
using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.FilterModels
{
    public class ServiceChanelFilterModel : RequestFilterModel
    {
        public ServiceChannelType ServiceChannelType { get; set; }
    }
}
