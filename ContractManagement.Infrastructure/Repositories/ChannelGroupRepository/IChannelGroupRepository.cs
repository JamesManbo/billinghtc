using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ChannelGroupRepository
{
    public interface IChannelGroupRepository : ICrudRepository<ChannelGroups, int>
    {
    }
}
