using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface ITemporaryGeneratingVoucherRepository : ICrudRepository<TemporaryGeneratingVoucher, int>
    {
        Task<int> JoinGenerated(DebtDbContext dbContext, IDbContextTransaction currentTransaction);
    }
    public class TemporaryGeneratingVoucherRepository : CrudRepository<TemporaryGeneratingVoucher, int>, ITemporaryGeneratingVoucherRepository
    {
        protected readonly DebtDbContext CurrentContext;
        public TemporaryGeneratingVoucherRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            this.CurrentContext = context;
        }

        public async Task<int> JoinGenerated(DebtDbContext dbContext, IDbContextTransaction currentTransaction)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(
                "CALL JoinGeneratedVoucherCategories"
                );
        }
    }
}
