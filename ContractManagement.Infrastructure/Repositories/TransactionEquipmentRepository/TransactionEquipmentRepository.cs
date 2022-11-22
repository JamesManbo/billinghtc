using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories
{
    public interface ITransactionEquipmentRepository : ICrudRepository<TransactionEquipment, int>
    {
    }
    public class TransactionEquipmentRepository : CrudRepository<TransactionEquipment, int>, ITransactionEquipmentRepository
    {
        public TransactionEquipmentRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
