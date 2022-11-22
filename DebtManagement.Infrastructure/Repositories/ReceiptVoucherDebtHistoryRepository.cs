using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IReceiptVoucherDebtHistoryRepository : ICrudRepository<ReceiptVoucherDebtHistory, int>
    {
        Task RollbackGenerate();
    }

    public class ReceiptVoucherDebtHistoryRepository : CrudRepository<ReceiptVoucherDebtHistory, int>, IReceiptVoucherDebtHistoryRepository
    {
        public ReceiptVoucherDebtHistoryRepository(DebtDbContext context,
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }

        public async Task RollbackGenerate()
        {
            var entities = DbSet.Where(c => c.IsAutomaticGenerate && EF.Functions.DateDiffDay(c.IssuedDate, DateTime.UtcNow) == 0);
            DbSet.RemoveRange(entities);
            await SaveChangeAsync();
        }
    }
}
