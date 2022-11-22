using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IVoucherAutoGenerateHistoryRepository : ICrudRepository<VoucherAutoGenerateHistory, int>
    {

    }
    public class VoucherAutoGenerateHistoryRepository : CrudRepository<VoucherAutoGenerateHistory, int>, IVoucherAutoGenerateHistoryRepository
    {
        public VoucherAutoGenerateHistoryRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) 
            : base(context, configAndMapper)
        {
        }
    }
}
