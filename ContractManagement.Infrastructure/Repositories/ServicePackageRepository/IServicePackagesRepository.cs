using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;

namespace ContractManagement.Infrastructure.Repositories.ServicePackageRepository
{
    public interface IServicePackagesRepository : ICrudRepository<ServicePackage, int>
    {
        bool CheckExitsServicePackageByName(string servicePackageName);
        bool CheckExitsServicePackageByCode(string servicePackageCode);
        int GetServicePackageIdByName(string servicePackageName);
        bool IsExistServicePackageByPrice(string servicePackageName, decimal price);
        bool CheckExitServicePackageName(string nameServicePackage, int id);
        bool CheckExitServicePackageCode(string codeServicePackage, int id);
    }
}
