using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using GenericRepository.Configurations;
using System.Linq;

namespace ContractManagement.Infrastructure.Repositories.ServiceGroupRepository
{
    public class ServiceGroupsRepository : CrudRepository<ServiceGroup, int>, IServiceGroupsRepository
    {
        public readonly ContractDbContext _contractDbContext;
        public ServiceGroupsRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public bool IsExistServiceGroup(string serviceGroupName) 
        {
            return _contractDbContext.ServiceGroups.Any(x => x.GroupName.Contains(serviceGroupName) && x.IsDeleted == false);
        }

        public int GetServiceGroupId(string serviceGroupName)
        {
            return _contractDbContext.ServiceGroups.Where(x => x.GroupName.Contains(serviceGroupName) && x.IsDeleted == false && x.IsActive == true).Select(y => y.Id).FirstOrDefault();
        }
    }
}
