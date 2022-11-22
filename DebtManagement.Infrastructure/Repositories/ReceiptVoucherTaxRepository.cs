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
    public interface IReceiptVoucherTaxRepository : ICrudRepository<ReceiptVoucherLineTax, int>
    {
        Task RollbackGenerate();
    }

    public class ReceiptVoucherTaxRepository : CrudRepository<ReceiptVoucherLineTax, int>, IReceiptVoucherTaxRepository
    {
        public ReceiptVoucherTaxRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context,
            configAndMapper)
        {
        }

        public async Task RollbackGenerate()
        {
            var entities = DbSet.Where(c => c.IsAutomaticGenerate && EF.Functions.DateDiffDay(c.CreatedDate, DateTime.UtcNow) == 0);
            DbSet.RemoveRange(entities);
            await SaveChangeAsync();
        }
    }
}
