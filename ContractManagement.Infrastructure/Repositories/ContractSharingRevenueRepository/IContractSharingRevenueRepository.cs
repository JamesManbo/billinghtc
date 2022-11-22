using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractSharingRevenueRepository
{
    public interface IContractSharingRevenueRepository : ICrudRepository<ContractSharingRevenueLine, int>
    {
    }
}
