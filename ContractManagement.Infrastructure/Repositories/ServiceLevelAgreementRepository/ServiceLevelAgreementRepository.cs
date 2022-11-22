using ContractManagement.Domain.AggregatesModel.BaseContract;
using GenericRepository;
using GenericRepository.Configurations;

namespace ContractManagement.Infrastructure.Repositories.ServiceLevelAgreementRepository
{
    public interface IServiceLevelAgreementRepository : ICrudRepository<ServiceLevelAgreement, int>
    {
    }
    public class ServiceLevelAgreementRepository : CrudRepository<ServiceLevelAgreement, int>, IServiceLevelAgreementRepository
    {
        private readonly ContractDbContext _context;
        public ServiceLevelAgreementRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }
    }
   
}
