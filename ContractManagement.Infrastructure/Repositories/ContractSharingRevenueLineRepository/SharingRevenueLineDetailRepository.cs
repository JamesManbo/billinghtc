using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository
{
    public interface ISharingRevenueLineDetailRepository : ICrudRepository<SharingRevenueLineDetail, int>
    {

    }
    public class SharingRevenueLineDetailRepository : CrudRepository<SharingRevenueLineDetail, int>, ISharingRevenueLineDetailRepository
    {
        public SharingRevenueLineDetailRepository(ContractDbContext context,
            IWrappedConfigAndMapper configAndMapper) 
            : base(context, configAndMapper)
        {
        }
    }
}
