using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IServicePackagePriceQueries : IQueryRepository
    {
        IPagedList<ServicePackagePriceDTO> GetList(RequestFilterModel requestFilterModel);
        ServicePackagePriceDTO Find(int id);
        public class ServicePackagePriceQueries : QueryRepository<ServicePackagePrice, int>, IServicePackagePriceQueries
        {
            public ServicePackagePriceQueries(ContractDbContext context) : base(context)
            {
            }

            public ServicePackagePriceDTO Find(int id)
            {
                var dapperExecution = BuildByTemplate<ServicePackagePriceDTO>();
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
                return dapperExecution.ExecuteScalarQuery();
            }

            public IPagedList<ServicePackagePriceDTO> GetList(RequestFilterModel requestFilterModel)
            {
                var dapperExecution = BuildByTemplate<ServicePackagePriceDTO>(requestFilterModel);
                return dapperExecution.ExecutePaginateQuery();
            }
        }
    }
}
