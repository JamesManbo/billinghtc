using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;

namespace ContractManagement.Infrastructure.Repositories.ServiceGroupRepository
{
    public interface IServiceGroupsRepository : ICrudRepository<ServiceGroup, int>
    {
        bool IsExistServiceGroup(string serviceGroupName);
        int GetServiceGroupId(string serviceGroupName);
    }
}
