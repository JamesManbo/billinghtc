using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using GenericRepository;
using GenericRepository.Configurations;

namespace ContractManagement.Infrastructure.Repositories.TransactionPromotionForContractRepositopy
{
    public interface ITransactionPromotionForContractRepository : ICrudRepository<TransactionPromotionForContract, int>
    {

    }
    public class TransactionPromotionForContractRepository : CrudRepository<TransactionPromotionForContract, int>, ITransactionPromotionForContractRepository
    {
        private readonly ContractDbContext _context;
        public TransactionPromotionForContractRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

       
    }
}
