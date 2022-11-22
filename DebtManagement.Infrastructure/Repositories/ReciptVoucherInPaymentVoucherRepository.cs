using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IReceiptVoucherInPaymentVoucherRepository : ICrudRepository<ReceiptVoucherInPaymentVoucher, int>
    {
      
    }

    public class ReciptVoucherInPaymentVoucherRepository : CrudRepository<ReceiptVoucherInPaymentVoucher, int>, IReceiptVoucherInPaymentVoucherRepository
    {
        public ReciptVoucherInPaymentVoucherRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
        }
    }
}
