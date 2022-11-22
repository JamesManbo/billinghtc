using Dapper;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.DashboardModels;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IDashboardQueries : IQueryRepository
    {
        IEnumerable<RevenueAndTaxDashboardModel> GetRevenueAndTaxAmountInYear();
        IEnumerable<DailyRevenueByServiceGroupModel> GetDailyRevenueByServiceGroup();
        IEnumerable<DailyRevenueByServiceModel> GetDailyRevenueByService();
    }
    public class DashboardQueries : QueryRepository<ReceiptVoucher, int>, IDashboardQueries
    {

        public DashboardQueries(DebtDbContext context) : base(context)
        {
        }

        public IEnumerable<RevenueAndTaxDashboardModel> GetRevenueAndTaxAmountInYear()
        {
            return  WithConnection(conn =>
               conn.Query<RevenueAndTaxDashboardModel>("GetRevenueAndTaxAmountInYear", commandType: CommandType.StoredProcedure));
        }

        public IEnumerable<DailyRevenueByServiceGroupModel> GetDailyRevenueByServiceGroup()
        {
            return WithConnection(conn =>
              conn.Query<DailyRevenueByServiceGroupModel>("sp_GetDailyRevenueByServiceGroup", commandType: CommandType.StoredProcedure));           
        }

        public IEnumerable<DailyRevenueByServiceModel> GetDailyRevenueByService()
        {
            return WithConnection(conn =>
              conn.Query<DailyRevenueByServiceModel>("sp_GetDailyRevenueByService", commandType: CommandType.StoredProcedure));
        }
    }
}
