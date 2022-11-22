using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Global.Models.StateChangedResponse;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IReceiptVoucherRepository : ICrudRepository<ReceiptVoucher, int>
    {
        bool IsExistByCode(string voucherCode, int? id = null);
        bool IsExistByInvoice(string invoiceCode, int? id = null);
        Task<List<ReceiptVoucher>> GetByIdsAsync(int[] arrId);
        Task<List<ReceiptVoucher>> GetByCodesAsync(string[] arrCode, int[] validStatus = null);
        Task<List<ReceiptVoucher>> GetUnpaidByTarget(int targetId);
        Task<ReceiptVoucher> GetByOutContractServicePackageAsync(OutContractServicePackageIntegrationEvent outContractServicePackageEvent);
        Task<ReceiptVoucher> GetIssuedDateNotValidVchr(int outContractSrvPckId);
        Task<ReceiptVoucher> GetByReceiptVoucherDetailAsync(int oCSPId, DateTime nextBilling);
        Task<List<ReceiptVoucher>> GetByReceiptVoucherDetailStartDateAsync(int oCSPId, DateTime startBillingDate);
        Task<List<ReceiptVoucher>> GetPendingByOCSPIdsGreaterNowAsync(List<int> oCSPIds);
        bool UpdateApplicationUserClass(string userIdentityGuid, int outContractId);
        Task<ActionResponse> ConfirmCollectionOnBehalfDebt(int[] rcptVoucherIds, string approvedBy, DateTime confirmationDate);
        bool ReceiptVouchersUpdateOverdueAndBadDebt(int numberDaysBadDebtInReceipt);
        Task RollbackGenerate();
        Task<List<ReceiptVoucher>> GetOverdueVouchers();
        Task<List<ReceiptVoucher>> GetDueVouchers();

        void UpdateRange(IEnumerable<ReceiptVoucher> updateModels);
    }

    public class ReceiptVoucherRepository : CrudRepository<ReceiptVoucher, int>, IReceiptVoucherRepository
    {
        public ReceiptVoucherRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context,
            configAndMapper)
        {
        }

        public bool IsExistByCode(string voucherCode, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(voucherCode))
            {
                return false;
            }

            return DbSet.Any(o =>
                o.VoucherCode.Equals(voucherCode, StringComparison.OrdinalIgnoreCase)
                && (!id.HasValue || !id.Equals(o.Id)) && !o.IsDeleted);
        }
        public bool IsExistByInvoice(string voucherCode, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(voucherCode))
            {
                return false;
            }

            return DbSet.Any(o =>
                o.InvoiceCode.Equals(voucherCode, StringComparison.OrdinalIgnoreCase)
                && (!id.HasValue || !id.Equals(o.Id)) && !o.IsDeleted);
        }

        public override Task<ReceiptVoucher> GetByIdAsync(object id)
        {
            return DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .ThenInclude(e => e.ReceiptVoucherDetailReductions)
                .Include(e => e.ReceiptVoucherDetails)
                .ThenInclude(e => e.PriceBusTables)
                .Include(e => e.ReceiptVoucherDetails)
                .ThenInclude(e => e.BusTablePricingCalculators)
                .Include(e => e.PromotionForReceiptVoucher)
                .Include(e => e.OpeningDebtHistories)
                .ThenInclude(d => d.PaymentDetails)
                .Include(e => e.DebtHistories)
                .ThenInclude(d => d.PaymentDetails)
                .Where(e => e.Id == (int)id).FirstOrDefaultAsync();
        }

        public Task<List<ReceiptVoucher>> GetByIdsAsync(int[] arrIds)
        {
            return DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .Include(e => e.PromotionForReceiptVoucher)
                .Include(e => e.OpeningDebtHistories)
                .ThenInclude(d => d.PaymentDetails)
                .Include(e => e.DebtHistories)
                .ThenInclude(d => d.PaymentDetails)
                .Where(e => arrIds.Contains(e.Id) && !e.IsDeleted).ToListAsync();
        }

        public Task<List<ReceiptVoucher>> GetByCodesAsync(string[] voucherCodes, int[] validStatus = null)
        {
            //var query = DbSet.Where(
            //        v => voucherCodes.Any(c => c.Equals(v.VoucherCode, StringComparison.OrdinalIgnoreCase))
            //    );
            var query = DbSet.Where(
                    v => voucherCodes.Any(c => c == v.VoucherCode)
                );

            if (validStatus != null && validStatus.Any())
            {
                query = query
                    .Where(v => validStatus.Contains(v.StatusId));
            }

            return query.ToListAsync();
        }

        public async Task<ReceiptVoucher> GetByOutContractServicePackageAsync(OutContractServicePackageIntegrationEvent outContractServicePackageEvent)
        {
            return await DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .FirstOrDefaultAsync(e => outContractServicePackageEvent.OutContractId == e.OutContractId
                && outContractServicePackageEvent.NextBilling <= e.IssuedDate && e.TypeId == ReceiptVoucherType.Billing.Id
                && e.StatusId == ReceiptVoucherStatus.Pending.Id);
        }

        public Task<ReceiptVoucher> GetIssuedDateNotValidVchr(int outContractSrvPckId)
        {
            return DbSet.FirstOrDefaultAsync(c => c.InvalidIssuedDate
                && c.TypeId == ReceiptVoucherType.Billing.Id
                && c.ReceiptVoucherDetails.Any(r => r.OutContractServicePackageId == outContractSrvPckId));
        }

        public Task<ReceiptVoucher> GetByReceiptVoucherDetailAsync(int oCSPId, DateTime nextBilling)
        {
            return DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .FirstOrDefaultAsync(e => e.ReceiptVoucherDetails.Any(a => a.OutContractServicePackageId == oCSPId
                    && a.StartBillingDate == nextBilling
                    && a.IsDeleted != true)
                && e.TypeId == ReceiptVoucherType.Billing.Id
                && e.StatusId == ReceiptVoucherStatus.Pending.Id);
        }

        public Task<List<ReceiptVoucher>> GetByReceiptVoucherDetailStartDateAsync(int oCSPId, DateTime startBillingDate)
        {
            return DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .Where(e => e.ReceiptVoucherDetails.Any(a => a.OutContractServicePackageId == oCSPId && a.StartBillingDate >= startBillingDate && a.IsDeleted != true)
                && e.StatusId != ReceiptVoucherStatus.Canceled.Id).ToListAsync();
        }

        public Task<List<ReceiptVoucher>> GetPendingByOCSPIdsGreaterNowAsync(List<int> oCSPIds)
        {
            return DbSet
                .Include(e => e.ReceiptVoucherDetails)
                .Include(e => e.OpeningDebtHistories)
                .Include(e => e.DebtHistories)
                .Where(e => e.ReceiptVoucherDetails.Any(a => oCSPIds.Contains(a.OutContractServicePackageId) && a.StartBillingDate > DateTime.Now && a.IsDeleted != true)
                && e.StatusId == ReceiptVoucherStatus.Pending.Id).ToListAsync();
        }


        public bool UpdateApplicationUserClass(string userIdentityGuid, int outContractId)
        {
            Context.Database.ExecuteSqlRaw("CALL UpdateApplicationUserClass(@userIdentityGuid, @outContractId)",
                new MySqlParameter("@userIdentityGuid", MySqlDbType.VarChar)
                {
                    Value = userIdentityGuid
                },
                new MySqlParameter("@outContractId", MySqlDbType.Int32)
                {
                    Value = outContractId
                });
            return true;
        }

        public async Task<ActionResponse> ConfirmCollectionOnBehalfDebt(int[] rcptVoucherIds, string approvedBy, DateTime confirmationDate)
        {
            var needConfirmVchrs = await this.GetByIdsAsync(rcptVoucherIds);
            foreach (var voucherEntity in needConfirmVchrs)
            {
                voucherEntity.SetStatusId(ReceiptVoucherStatus.Success.Id);
                voucherEntity.ApprovedUserId = approvedBy;
                voucherEntity.PaymentDate = confirmationDate;
                voucherEntity.CalculatePaidTotal();
            }
            try
            {
                DbSet.UpdateRange(needConfirmVchrs);
            }
            catch (Exception x)
            {
                throw x;
            }


            return ActionResponse.Success;
        }

        public async Task<List<ReceiptVoucher>> GetUnpaidByTarget(int targetId)
        {
            if (targetId == default)
            {
                return null;
            }

            return await DbSet
                .Where(r => targetId == r.TargetId)
                .Include(r => r.DebtHistories)
                .ToListAsync();
        }


        public bool ReceiptVouchersUpdateOverdueAndBadDebt(int numberDaysBadDebtInReceipt)
        {
            Context.Database.ExecuteSqlRaw("CALL ReceiptVouchersUpdateOverdueAndBadDebt(@numberDaysBadDebtInReceipt)",
                new MySqlParameter("@numberDaysBadDebtInReceipt", MySqlDbType.Int32)
                {
                    Value = numberDaysBadDebtInReceipt
                });

            return true;
        }

        public async Task RollbackGenerate()
        {
            var rollbackDate = DateTime.UtcNow.AddHours(7);
            var entities = DbSet.Where(c => c.IsAutomaticGenerate && EF.Functions.DateDiffDay(c.IssuedDate, rollbackDate) == 0);
            DbSet.RemoveRange(entities);
            await SaveChangeAsync();
        }

        public Task<List<ReceiptVoucher>> GetOverdueVouchers()
        {
            return
                DbSet.Where(c => c.StatusId == ReceiptVoucherStatus.Overdue.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy những phiếu thu đã quá ngày phải thanh toán
        /// Lưu ý với hàm EF.Functions.DateDiffDay(@t1, @t2):
        /// hàm này sẽ đếm số ngày trong phạm vi từ @t1 đến @t2,
        /// nếu @t1 nhỏ hơn @t2 => trả về số ngày = @t2 - @t1
        /// nếu @t1 lớn hơn @t2 => trả về số âm = @t2 - @t1
        /// </summary>
        /// <returns></returns>
        public Task<List<ReceiptVoucher>> GetDueVouchers()
        {
            var dueStatuses = ReceiptVoucherStatus.UnpaidStatuses().Where(s => s != ReceiptVoucherStatus.Overdue.Id);
            var entities = DbSet.Where(c => dueStatuses.Contains(c.StatusId)
                && ((!c.IsEnterprise && EF.Functions.DateDiffDay(c.IssuedDate, DateTime.UtcNow.AddHours(7)) > c.NumberBillingLimitDays)
                || (c.IsEnterprise && EF.Functions.DateDiffDay(c.InvoiceDate, DateTime.UtcNow.AddHours(7)) > c.NumberBillingLimitDays))
                && !c.IsDeleted);

            return entities.ToListAsync();
        }

        public void UpdateRange(IEnumerable<ReceiptVoucher> updateModels)
        {
            DbSet.UpdateRange(updateModels);
        }
    }
}