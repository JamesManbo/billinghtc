using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.PromotionRepository
{
    public interface IPromotionProductRepository : ICrudRepository<PromotionProduct, int>
    {
        Task<ActionResponse> UpdatePromotionProduct(int NewId, int OldId);
    }

    public class PromotionProductRepository : CrudRepository<PromotionProduct, int>, IPromotionProductRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public PromotionProductRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public async Task<ActionResponse> UpdatePromotionProduct(int newId, int oldId)
        {
                string sQuery = $"CALL sp_UpdatePromotionProduct('{oldId}','{newId}')";
                await _contractDbContext.Database.ExecuteSqlRawAsync(sQuery);
                await _contractDbContext.SaveChangesAsync();
                return ActionResponse.Success;
        }
    }
}
