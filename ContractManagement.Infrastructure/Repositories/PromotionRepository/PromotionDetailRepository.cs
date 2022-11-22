using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.PromotionRepository
{
    public interface IPromotionDetailRepository : ICrudRepository<PromotionDetail, int>
    {
        Task<List<PromotionDetail>> GetByPromotionId(int promotionId);
    }

    public class PromotionDetailRepository : CrudRepository<PromotionDetail, int>, IPromotionDetailRepository
    {
        public PromotionDetailRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {

        }

        public Task<List<PromotionDetail>> GetByPromotionId(int promotionId)
        {
            return DbSet.Where(p => p.PromotionId == promotionId && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
