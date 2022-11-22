using System.Collections.Generic;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using Global.Models.StateChangedResponse;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IOutContractRepository : ICrudRepository<OutContract, int>
    {
        Task<List<OutContract>> GetByTransactionIdsAsync(int[] ids);
        Task<int> AutoRenewExpirationContract();
    }
}
