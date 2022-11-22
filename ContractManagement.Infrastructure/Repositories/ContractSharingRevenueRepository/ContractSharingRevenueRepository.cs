using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractSharingRevenueRepository
{
    public class ContractSharingRevenueRepository : CrudRepository<ContractSharingRevenueLine, int>, IContractSharingRevenueRepository
    {
        public ContractSharingRevenueRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
        }
    }
}
