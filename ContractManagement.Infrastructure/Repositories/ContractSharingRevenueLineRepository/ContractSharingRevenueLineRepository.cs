using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository
{
    public class ContractSharingRevenueLineRepository : CrudRepository<ContractSharingRevenueLine, int>, IContractSharingRevenueLineRepository
    {
        public ContractSharingRevenueLineRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
        }

        public void DeleteMany(List<int> deleteIds)
        {
            if (deleteIds.Count == 0) return;

            Context.Database.ExecuteSqlRaw($"DELETE FROM {TableName} WHERE Id IN ({string.Join(",", deleteIds)})");
        }

        public bool MapContractSharingRevenueLineToHead()
        {
            Context.Database.ExecuteSqlRaw("CALL MapContractSharingRevenueLineToHead");
            return true;
        }
    }
}
