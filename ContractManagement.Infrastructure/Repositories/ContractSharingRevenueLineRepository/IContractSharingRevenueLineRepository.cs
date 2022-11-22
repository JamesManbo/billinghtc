using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository
{
    public interface IContractSharingRevenueLineRepository : ICrudRepository<ContractSharingRevenueLine, int>
    {
        bool MapContractSharingRevenueLineToHead();
        void DeleteMany(List<int> deleteIds);
    }
}
