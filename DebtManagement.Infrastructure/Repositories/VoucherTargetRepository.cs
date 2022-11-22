using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IVoucherTargetRepository : ICrudRepository<VoucherTarget, int>
    {
        Task UpdateAccountingCodes();
        Task<VoucherTarget> FindVoucherTargetByIdentityGuid(string identityGuid);
        Task<bool> UpdateTargetCurrentDebt(int targetId, decimal debtAmount, bool increase = true);
        Task<int> UpdateCurrentDebtForAllTarget(DebtDbContext dbContext, IDbContextTransaction currentTransaction);
    }

    public class VoucherTargetRepository : CrudRepository<VoucherTarget, int>, IVoucherTargetRepository
    {
        protected readonly DebtDbContext CurrentContext;
        public VoucherTargetRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            this.CurrentContext = context;
        }

        public Task<VoucherTarget> FindVoucherTargetByIdentityGuid(string identityGuid)
        {
            return DbSet.FirstOrDefaultAsync(c => c.IdentityGuid.Equals(identityGuid));
        }

        public async Task<bool> UpdateTargetCurrentDebt(int targetId, decimal debtAmount, bool increase = true)
        {
            var currentTarget = await this.GetByIdAsync(targetId);
            if (currentTarget == null)
            {
                return false;
            }
            if (increase)
            {
                currentTarget.CurrentDebt += debtAmount;
            }
            else
            {
                currentTarget.CurrentDebt -= debtAmount;
            }
            await SaveChangeAsync();

            return true;
        }

        public async Task<int> UpdateCurrentDebtForAllTarget(DebtDbContext dbContext, IDbContextTransaction currentTransaction)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(
                "CALL UpdateCurrentDebtOfTarget"
                );
        }

        public async Task UpdateAccountingCodes()
        {
            await DbContext.Database.ExecuteSqlRawAsync("CALL UpdateAccountingCodes");
        }
    }
}
