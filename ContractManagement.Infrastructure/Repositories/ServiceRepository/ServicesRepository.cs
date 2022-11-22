using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using GenericRepository.Configurations;
using System.Linq;

namespace ContractManagement.Infrastructure.Repositories.ServiceRepository
{
    public class ServicesRepository : CrudRepository<Service, int>, IServicesRepository
    {
        public readonly ContractDbContext _contractDbContext;
        public ServicesRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool CheckExitsServiceByName(string serviceName)
        {
            return _contractDbContext.Services.Any(x => x.ServiceName.Contains(serviceName) && x.IsDeleted == false );
        }

        public int GetServiceIdByName(string serviceName)
        {
            return _contractDbContext.Services.Where(x => x.ServiceName.Contains(serviceName) && x.IsDeleted == false && x.IsActive == true).Select(y => y.Id).FirstOrDefault();
        }
    }
}
