using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IPromotionForReceiptVoucherRepository : ICrudRepository<PromotionForReceiptVoucher, int>
    {
    }

    public class PromotionForReceiptVoucherRepositories : CrudRepository<PromotionForReceiptVoucher, int>, IPromotionForReceiptVoucherRepository
    {
        public PromotionForReceiptVoucherRepositories(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
