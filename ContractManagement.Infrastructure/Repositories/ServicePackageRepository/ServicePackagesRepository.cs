using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.ServicePackageRepository
{
    public class ServicePackagesRepository : CrudRepository<ServicePackage, int>, IServicePackagesRepository
    {
        private readonly ContractDbContext _context;
        public ServicePackagesRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }

        public override async Task<ServicePackage> GetByIdAsync(object id)
        {
            return await DbSet
                .Include(e => e.ServicePackageRadiusServices)
                .Where(e => e.Id == (int) id).FirstOrDefaultAsync();
        }

        public bool CheckExitsServicePackageByName( string servicePackageName)
        {
            return _context.ServicePackages.Any(x => x.PackageName.Contains(servicePackageName) && x.IsDeleted == false && x.IsActive == true);
        }

        public bool CheckExitsServicePackageByCode(string servicePackageCode)
        {
            return _context.ServicePackages.Any(x => x.PackageCode.Contains(servicePackageCode) && x.IsDeleted == false && x.IsActive == true);
        }

        public bool CheckExitServicePackageName(string nameServicePackage, int id)
        {
            return _context.ServicePackages
                .Any(x => x.PackageName == nameServicePackage.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExitServicePackageCode(string codeServicePackage, int id)
        {
            return _context.ServicePackages
                .Any(x => x.PackageCode == codeServicePackage.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public int GetServicePackageIdByName(string servicePackageName)
        {
            return _context.ServicePackages.Where(x => x.PackageName.Contains(servicePackageName) && x.IsDeleted == false && x.IsActive == true).Select(y => y.Id).FirstOrDefault();
        }

        public bool IsExistServicePackageByPrice(string servicePackageName ,decimal price)
        {
            return _context.ServicePackages.Any(x => x.PackageName.Contains(servicePackageName) && x.Price != price && x.IsDeleted == false && x.IsActive == true);
        }
    }
}
