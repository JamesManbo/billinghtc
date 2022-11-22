using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IExchangeRateQueries : IQueryRepository
    {
        IEnumerable<ExchangeRateDTO> GetAllInDate(DateTime dateTime);
    }
    public class ExchangeRateQueries : QueryRepository<ExchangeRate, int>, IExchangeRateQueries
    {
        public ExchangeRateQueries(ContractDbContext context) : base(context)
        {

        }

        public IEnumerable<ExchangeRateDTO> GetAllInDate(DateTime date)
        {
            var dapperExecution = BuildByTemplate<ExchangeRateDTO>();

            dapperExecution.SqlBuilder.Where("DATE(t1.CreatedDate) = DATE(@date1)", new { date1 = date });

            return dapperExecution.ExecuteQuery();
        }
    }
}
