using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.InContractRepository
{
    public interface IInContractRepository : ICrudRepository<InContract, int>
    {
        bool DeleteContractSharingRevenuesByIds(int[] contractSharingRevenueIds, string updateBy);
    }
}
