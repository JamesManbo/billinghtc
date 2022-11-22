using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using Global.Models.StateChangedResponse;

namespace ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository
{
    public interface IServicePackagePriceRepository : ICrudRepository<ServicePackagePrice, int>
    {
        Task<ActionResponse> DeleteAndSaveByPackageIdAsync(int packageId);
        Task<ActionResponse> DeleteWithListId(string listId, int id);
    }
}
