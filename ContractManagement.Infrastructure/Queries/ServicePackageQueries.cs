using CachingLayer.Interceptor;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Domain.Models.RadiusAndBras;
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
    public interface IServicePackageQueries : IQueryRepository
    {
        [Cache]
        IEnumerable<ServicePackageSimpleDTO> GetAllSimple(RequestFilterModel requestFilterModel = null);
        [Cache]
        IEnumerable<ServicePackageDTO> GetAll(RequestFilterModel requestFilterModel = null);
        IPagedList<ServicePackageDTO> GetList(RequestFilterModel requestFilterModel);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        IEnumerable<ServicePackageDTO> GetListByServiceId(int serviceId);
        IPagedList<ServicePackageDTO> GetPackagesByServiceId(PackageFilterModel requestFilterModel);
        IEnumerable<ServicePackageDTO> FindByIds(int[] ids);
        ServicePackageDTO Find(int id);
        ServicePackageRadiusServiceDTO FindRadiusService(int servicePackageId, int radiusServerId);
        int GetLatestId();
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
    }

    public class ServicePackageQueries : QueryRepository<ServicePackage, int>, IServicePackageQueries
    {
        public ServicePackageQueries(ContractDbContext context) : base(context)
        {
        }
        public ServicePackageDTO Find(int id)
        {
            // ServicePackageDTO cache = null;
            var cached = new Dictionary<int, ServicePackageDTO>();
            var dapperExecution = BuildByTemplate<ServicePackageDTO>();
            dapperExecution.SqlBuilder.Select("t2.ServiceName AS ServiceName");
            dapperExecution.SqlBuilder.Select("t4.PackageName AS ParentName");
            dapperExecution.SqlBuilder.Select("t4.PackageCode AS ParentCode");

            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.ServicePackageId");
            dapperExecution.SqlBuilder.Select("t3.RadiusServerId");
            dapperExecution.SqlBuilder.Select("t3.RadiusServiceId");

            dapperExecution.SqlBuilder.InnerJoin("Services t2 ON t2.Id = t1.ServiceId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("ServicePackageRadiusServices t3 ON t3.ServicePackageId = t1.Id AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("ServicePackages t4 ON t1.ParentId = t4.Id AND t4.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery<ServicePackageDTO, ServicePackageRadiusServiceDTO>(
                (servicepackage, srvPackageRadiusSrv) =>
                {
                    if (!cached.TryGetValue(servicepackage.Id, out var servicePackageModel))
                    {
                        servicePackageModel = servicepackage;
                    }

                    if (srvPackageRadiusSrv != null
                        && servicepackage.ServicePackageRadiusServices.All(s => s.Id != srvPackageRadiusSrv.Id))
                    {
                        servicePackageModel.ServicePackageRadiusServices.Add(srvPackageRadiusSrv);
                    }
                    return servicePackageModel;

                }, "Id");
        }

        public IEnumerable<ServicePackageSimpleDTO> GetAllSimple(RequestFilterModel requestFilterModel = null)
        {
            requestFilterModel = requestFilterModel ?? new RequestFilterModel();
            requestFilterModel.Paging = false;
            var dapperExecution = BuildByTemplate<ServicePackageSimpleDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t1.PackageName AS `DisplayName`");

            return dapperExecution.ExecuteQuery();
        }
        public IEnumerable<ServicePackageDTO> GetAll(RequestFilterModel requestFilterModel = null)
        {
            requestFilterModel = requestFilterModel ?? new RequestFilterModel();
            requestFilterModel.Paging = false;
            var dapperExecution = BuildByTemplate<ServicePackageDTO>(requestFilterModel);
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ServicePackageDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t2.ServiceName");
            dapperExecution.SqlBuilder.Select("CONCAT(t1.InternationalBandwidth,' ',t1.InternationalBandwidthUom) AS InternationalBandwidthLabel");
            dapperExecution.SqlBuilder.Select("CONCAT(t1.DomesticBandwidth,' ',t1.DomesticBandwidthUom) AS DomesticBandwidthLabel");
            dapperExecution.SqlBuilder.LeftJoin("Services t2 ON t1.ServiceId = t2.Id");
            // dapperExecution.SqlBuilder.LeftJoin("ServicePackagePrice t3 ON t1.Id = t3.ChannelId");
            // dapperExecution.SqlBuilder.Where("IFNULL(t3.IsActive ,1)= 1 AND IFNULL(t3.IsDeleted, 0) = 0 AND IFNULL(t2.IsActive, 1) = 1 AND IFNULL(t2.IsDeleted, 0) = 0");

            // return dapperExecution.ExecutePaginateQuery();
            // dapperExecution.SqlBuilder.LeftJoin("ServicePackagePrice t3 ON t1.Id = t3.ChannelId");

            if (requestFilterModel.Any("ServicePackagePrice"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "ServicePackagePrice");
                dapperExecution.SqlBuilder.AppendPredicate<decimal>("t3.PriceValue", propertyFilter);
            }

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.ServiceId AS `ParentId`");
            dapperExecution.SqlBuilder.Select("t1.PackageName AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            return dapperExecution.ExecuteQuery();
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>();
            dapperExecution.SqlBuilder.Where("t1.PackageCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>();
            dapperExecution.SqlBuilder.Where("t1.PackageName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public IEnumerable<ServicePackageDTO> GetListByServiceId(int serviceId)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>();
            dapperExecution.SqlBuilder.Where("t1.ServiceId = @serviceId", new { serviceId });
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ServicePackageDTO> GetPackagesByServiceId(PackageFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.LeftJoin("Services t2 ON t1.ServiceId = t2.Id");
            dapperExecution.SqlBuilder.Select("t2.ServiceName");
            dapperExecution.SqlBuilder.Where("t1.ServiceId = @serviceId", new { requestFilterModel.ServiceId });
            if (requestFilterModel.OnlyRoot)
            {
                dapperExecution.SqlBuilder.Where("t1.ParentId IS NULL");
            }
            
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<ServicePackageDTO> FindByIds(int[] ids)
        {
            var dapperExecution = BuildByTemplate<ServicePackageDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            return dapperExecution.ExecuteQuery();
        }

        public ServicePackageRadiusServiceDTO FindRadiusService(int servicePackageId, int radiusServerId)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<ServicePackageRadiusServiceDTO>();
            dapperExecution.SqlBuilder.Select("IFNULL(m.Id, 0) AS Id");
            dapperExecution.SqlBuilder.Select("IFNULL(m.RadiusServiceId, 0) AS RadiusServiceId");
            dapperExecution.SqlBuilder.Select("IFNULL(m.RadiusServerId, 0) AS RadiusServerId");
            dapperExecution.SqlBuilder.Select("rad.ServerName AS RadiusServerName");
            dapperExecution.SqlBuilder.Select("t1.Id AS ServicePackageId");
            dapperExecution.SqlBuilder.Select("t1.PackageName AS BillingPackageName");

            dapperExecution.SqlBuilder.LeftJoin("ServicePackageRadiusServices m ON m.ServicePackageId = t1.Id AND m.RadiusServerId = @radiusServerId", new { radiusServerId });
            dapperExecution.SqlBuilder.LeftJoin("RadiusServerInformation rad ON rad.Id = m.RadiusServerId");
            dapperExecution.SqlBuilder.Where("t1.Id = @servicePackageId", new { servicePackageId });
            return dapperExecution.ExecuteScalarQuery();
        }

        public int GetLatestId()
        {
            return WithConnection<int>(conn => conn.ExecuteScalar<int>(
                   $"SELECT Id FROM {this.TableName} ORDER BY Id DESC LIMIT 1"));
        }
    }
}