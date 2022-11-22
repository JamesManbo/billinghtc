using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using GenericRepository;
using GenericRepository.Configurations;

namespace ContractManagement.Infrastructure.Repositories.PromotionRepository
{
    public interface IPromotionForContractRepository : ICrudRepository<PromotionForContract, int>
    {

    }
    public class PromotionForContractRepository : CrudRepository<PromotionForContract, int>, IPromotionForContractRepository
    {
        public PromotionForContractRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {

        }
    }
}
