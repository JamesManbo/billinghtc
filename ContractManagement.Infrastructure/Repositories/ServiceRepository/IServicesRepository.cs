using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;

namespace ContractManagement.Infrastructure.Repositories.ServiceRepository
{
    public interface IServicesRepository : ICrudRepository<Service, int>
    {
        bool CheckExitsServiceByName(string serviceName);
        int GetServiceIdByName(string serviceName);
    }
}
