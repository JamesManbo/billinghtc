using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ChannelGroupRepository
{
    public class ChannelGroupRepository : CrudRepository<ChannelGroups, int>, IChannelGroupRepository
    {
        private readonly ContractDbContext _context;
        public ChannelGroupRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }
    }
}
