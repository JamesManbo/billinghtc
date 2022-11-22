using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IReceiptVoucherDetailRepository : ICrudRepository<ReceiptVoucherDetail, int>
    {
        Task<ReceiptVoucherDetail> FindFirstReceiptDetail(int outContractSrvPackageId);
        Task RollbackGenerate();
    }
    public class ReceiptVoucherDetailRepository : CrudRepository<ReceiptVoucherDetail, int>, IReceiptVoucherDetailRepository
    {
        public ReceiptVoucherDetailRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }

        public Task<ReceiptVoucherDetail> FindFirstReceiptDetail(int outContractSrvPackageId)
        {
            return DbSet.FirstOrDefaultAsync(e =>
                e.OutContractServicePackageId == outContractSrvPackageId &&
                e.ReceiptVoucher.TypeId == ReceiptVoucherType.Signed.Id);
        }

        public async Task RollbackGenerate()
        {
            var entities = DbSet.Where(c => c.IsAutomaticGenerate && EF.Functions.DateDiffDay(c.CreatedDate, DateTime.UtcNow) == 0);
            DbSet.RemoveRange(entities);
            await SaveChangeAsync();
        }
    }
}
