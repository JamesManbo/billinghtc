using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IPaymentVoucherDetailRepository : ICrudRepository<PaymentVoucherDetail, int>
    {
    }

    public class PaymentVoucherDetailRepository : CrudRepository<PaymentVoucherDetail, int>, IPaymentVoucherDetailRepository
    {
        public PaymentVoucherDetailRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }
    }
}
