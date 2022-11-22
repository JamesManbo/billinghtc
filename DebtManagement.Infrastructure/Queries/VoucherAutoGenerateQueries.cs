using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IVoucherAutoGenereateQueries : IQueryRepository
    {

    }
    public class VoucherAutoGenerateQueries : QueryRepository<VoucherAutoGenerateHistory, int>, IVoucherAutoGenereateQueries
    {
        public VoucherAutoGenerateQueries(DebtDbContext context) : base(context)
        {
        }
    }
}
