using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.PromotionRepository
{

    public interface IPromotionTypeRepository : ICrudRepository<PromotionType, int>
    {
       
    }

    public class PromotionTypeRepository : CrudRepository<PromotionType, int>, IPromotionTypeRepository
    {
        public PromotionTypeRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
           
        }
      
    }
}
