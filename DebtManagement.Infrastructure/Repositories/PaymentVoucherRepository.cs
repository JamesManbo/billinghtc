using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using Microsoft.EntityFrameworkCore;
using Global.Models.StateChangedResponse;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IPaymentVoucherRepository : ICrudRepository<PaymentVoucher, int>
    {
        Task<List<PaymentVoucher>> GetByIdsAsync(int[] arrId);
        bool IsExistByCode(string voucherCode, int? id = null);
        Task<ActionResponse> ConfirmPayment(int[] voucherIds, bool approved = true, string approvalUserId = "", string reasonContent = "");
        bool PaymentVouchersUpdateOverdue();
    }

    public class PaymentVoucherRepository : CrudRepository<PaymentVoucher, int>, IPaymentVoucherRepository
    {
        private readonly DebtDbContext _context;
        public PaymentVoucherRepository(DebtDbContext context, 
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public override Task<PaymentVoucher> GetByIdAsync(object id)
        {
            return DbSet.Include(e => e.PaymentVoucherDetails)
                .ThenInclude(d => d.PaymentVoucherLineTaxes)
                .Include(e => e.PaymentVoucherTaxes)
                .Include(e => e.PaymentDetails)
                .Where(e => e.Id == (int)id).FirstOrDefaultAsync();
        }

        public bool IsExistByCode(string voucherCode, int? id = null)
        {
            return _context.PaymentVouchers.Any(o => o.VoucherCode.Trim().Equals(voucherCode.Trim(), StringComparison.OrdinalIgnoreCase)
                    && (!id.HasValue || !id.Equals(o.Id)) && !o.IsDeleted);
        }

        public Task<List<PaymentVoucher>> GetByIdsAsync(int[] arrIds)
        {
            return DbSet
                .Where(e => arrIds.Contains(e.Id)).ToListAsync();
        }

        public async Task<ActionResponse> ConfirmPayment(int[] voucherIds, bool approved = true, string approvalUserId = "", string reasonContent = "")
        {
            var needConfirmVchrs = DbSet.Include(r => r.PaymentDetails).Where(r => voucherIds.Contains(r.Id));
            foreach (var voucherEntity in needConfirmVchrs)
            {
                voucherEntity.SetStatusId(
                    approved ? PaymentVoucherStatus.Success.Id : PaymentVoucherStatus.Rejected.Id,
                    approvalUserId);

                if(voucherEntity.PaymentDetails.Any(p => p.PaidAmount > 0))
                {
                    voucherEntity.CashTotal = voucherEntity.PaymentDetails.Sum(p => p.PaidAmount);
                    voucherEntity.PaidTotal = voucherEntity.CashTotal + voucherEntity.ClearingTotal;
                }
                else
                {
                    var defaultPaymentDetail = voucherEntity.PaymentDetails.First(p => p.PaymentMethod == 0);
                    defaultPaymentDetail.PaidAmount = voucherEntity.GrandTotal;
                    voucherEntity.UpdatePaymentDetail(defaultPaymentDetail);
                    voucherEntity.CashTotal = voucherEntity.GrandTotal - voucherEntity.ClearingTotal;
                    voucherEntity.PaidTotal = voucherEntity.GrandTotal;
                }

                voucherEntity.CancellationReason = reasonContent;
            }

            DbSet.UpdateRange(needConfirmVchrs);
            await Context.SaveChangesAsync();
            return ActionResponse.Success;
        }

        public bool PaymentVouchersUpdateOverdue()
        {
            Context.Database.ExecuteSqlRaw("CALL PaymentVouchersUpdateOverdue()");

            return true;
        }
    }
}
