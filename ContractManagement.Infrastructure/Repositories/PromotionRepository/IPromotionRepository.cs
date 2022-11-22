using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System.Linq;

namespace ContractManagement.Infrastructure.Repositories.PromotionRepository
{
    public interface IPromotionRepository : ICrudRepository<Promotion, int>
    {
        bool CheckExitPromotionCode(string codePromotion, int id);
    }

    public class PromotionRepository : CrudRepository<Promotion, int>, IPromotionRepository
    {
        private readonly ContractDbContext _context;
        public PromotionRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public bool CheckExitPromotionCode(string codePromotion, int id)
        {
            return _context.Promotions.Any(x => x.PromotionCode == codePromotion.Trim() && x.IsDeleted == false && x.Id != id);
        }
    }
}
