using CachingLayer.Interceptor;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.RadiusAndBras;
using ContractManagement.RadiusDomain.Models;
using Dapper;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IRadiusServicePackageQueries : IQueryRepository
    {
        //ServicePackageRadiusServiceDTO GetRmServiceFromBilling(int radiusServerId, int billingPackageId);
    }

    public class RadiusServicePackageQueries : QueryRepository<ServicePackageRadiusService, int>, IRadiusServicePackageQueries
    {
        public RadiusServicePackageQueries(ContractDbContext context) : base(context)
        {
        }

        public ServicePackageRadiusServiceDTO GetRmServiceFromBilling(int radiusServerId, int billingPackageId)
        {
            var dapperExecution = BuildByTemplate<ServicePackageRadiusServiceDTO>();
            dapperExecution.SqlBuilder.Where("t1.ServicePackageId = @billingPackageId",
                new
                {
                    billingPackageId
                });
            dapperExecution.SqlBuilder.Where("t1.RadiusServerId = @radiusServerId", new { radiusServerId });
            return dapperExecution.ExecuteScalarQuery();
        }
    }
}