using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IClearingRepository : ICrudRepository<Clearing, int>
    {
        bool IsExistByCode(string voucherCode, string id = null);
        bool LockVoucher(List<string> voucherIds, bool isReceipt, string updatedBy);
        bool ClearingSuccessVoucher(List<string> voucherIds, bool isReceipt, string updatedBy, string clearingId);
    }
    public class ClearingRepository : CrudRepository<Clearing, int>, IClearingRepository
    {
        private readonly DebtDbContext _context;
        public ClearingRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }
        public override Task<Clearing> GetByIdAsync(object id)
        {
            IQueryable<Clearing> query;
            if (id is int)
            {
                query = DbSet.Where(e => e.Id == (int)id);
            }
            else
            {
                query = DbSet
                .Where(e => e.IdentityGuid == (string)id);
            }
            return query
                .Include(c => c.ReceiptVouchers)
                .Include(c => c.PaymentVouchers)
                .FirstOrDefaultAsync();
        }

        public bool IsExistByCode(string codeClearing, string id = null)
        {
            return DbSet.Any(o => !string.IsNullOrEmpty(codeClearing) && o.CodeClearing.Trim().Equals(codeClearing.Trim(), StringComparison.OrdinalIgnoreCase)
            && o.IdentityGuid != id && !o.IsDeleted);
        }

        public bool LockVoucher(List<string> voucherIds, bool isReceipt, string updatedBy)
        {
            if (voucherIds.Count == 0) return true;

            var affectedRecords = this._context.Database.ExecuteSqlRaw("CALL LockVoucher(@voucherIds, @isReceipt, @updatedBy)",
                    new MySqlParameter("@voucherIds", MySqlDbType.MediumText) { Value = string.Join(",", voucherIds) },
                    new MySqlParameter("@isReceipt", MySqlDbType.Bool) { Value = isReceipt },
                    new MySqlParameter("@updatedBy", MySqlDbType.VarChar) { Value = updatedBy });

            return affectedRecords > 0;
        }

        public bool ClearingSuccessVoucher(List<string> voucherIds, bool isReceipt, string updatedBy, string clearingId)
        {
            if (voucherIds.Count == 0) return true;

            var affectedRecords = _context.Database.ExecuteSqlRaw("CALL ClearingSuccessVoucher(@voucherIds, @isReceipt, @updatedBy, @clearingId)",
                     new MySqlParameter("@voucherIds", MySqlDbType.MediumText) { Value = string.Join(",", voucherIds) },
                     new MySqlParameter("@isReceipt", MySqlDbType.Bool) { Value = isReceipt },
                     new MySqlParameter("@updatedBy", MySqlDbType.VarChar) { Value = updatedBy },
                     new MySqlParameter("@clearingId", MySqlDbType.VarChar) { Value = clearingId });

            return affectedRecords > 0;
        }
    }
}
