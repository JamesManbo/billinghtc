using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Models;
using Dapper;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.FilterModels;
using AutoMapper.QueryableExtensions;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IContractorQueries : IQueryRepository
    {
        int FindIdByGuid(string uuid);
        ContractorDTO FindByContractorId(int id);
        ContractorDTO FindById(int id);
        ContractorDTO FindById(string id);
        IEnumerable<ContractorDTO> GetAllByIds(int[] ids);
        IEnumerable<ContractorDTO> GetFromId(int fromId);
        IEnumerable<ContractorDTO> GetAll();
        IEnumerable<SelectionItem> Autocomplete(RequestFilterModel requestFilterModel);
        IPagedList<ContractorDTO> GetList(RequestFilterModel filterModel);
        IPagedList<ContractorDTO> GetListByProjectIds(ContractorByProjectIdsFilterModel filterModel);
        IPagedList<ContractorDTO> GetListByProjectIdsForApp(ContractorByProjectIdsFilterModel filterModel);
        IPagedList<ContractorDTO> GetListByMarketIdsProjectIds(ContractorByMarketAreaIdsProjectIdsFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList();
        int GetContractorCountByProjectId(int projectId);
        int GetTotalNumber();
        int GetLatestId();
    }

    public class ContractorQueries : QueryRepository<Contractor, int>, IContractorQueries
    {
        public ContractorQueries(ContractDbContext contractDbContext) : base(contractDbContext)
        {
        }

        public int FindIdByGuid(string uid)
        {
            var dapperExe = BuildByTemplateWithoutSelect<int>();
            dapperExe.SqlBuilder.Select("t1.Id");
            dapperExe.SqlBuilder.Where("t1.IdentityGuid = @uid", new { uid });

            return dapperExe.ExecuteScalarQuery();
        }

        public ContractorDTO FindById(int id)
        {
            var dapperExe = BuildByTemplate<ContractorDTO>();
            dapperExe.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExe.ExecuteScalarQuery();
        }

        public ContractorDTO FindById(string id)
        {
            var dapperExe = BuildByTemplate<ContractorDTO>();
            dapperExe.SqlBuilder.Where("t1.IdentityGuid = @id", new { id });

            return dapperExe.ExecuteScalarQuery();
        }

        public ContractorDTO FindByContractorId(int id)
        {
            var dapperExe = BuildByTemplate<ContractorDTO>();
            dapperExe.SqlBuilder.InnerJoin("OutContracts oc ON oc.ContractorId = t1.Id");
            dapperExe.SqlBuilder.Where("oc.Id = @id", new { id });

            return dapperExe.ExecuteScalarQuery();
        }

        public IEnumerable<SelectionItem> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem>();
            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', t1.ContractorFullName, ', SĐT: ', t1.ContractorPhone , ', Đ/c: ', t1.ContractorAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractorCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorFullName LIKE @keywords",
                        new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorAddress LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorEmail LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorTaxIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.ContractorFax LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.AccountingCustomerCode LIKE @keywords",
                        new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQuery();
        }

        public List<OutContractDTO> GetExpired()
        {
            var cache = new Dictionary<int, OutContractDTO>();
            return WithConnection((conn) =>
                    conn.Query<OutContractDTO, PaymentMethod, ContractTimeLine, OutContractDTO>(
                        "GetExpiredOutContract",
                        (outContract, paymentMethod, contractTimeLine) =>
                        {
                            if (!cache.TryGetValue(outContract.Id, out var result))
                            {
                                result = outContract;
                                cache.Add(outContract.Id, result);
                            }

                            result.Payment = paymentMethod;
                            result.TimeLine = contractTimeLine;
                            return result;
                        },
                        commandType: CommandType.StoredProcedure,
                        splitOn: "Payment_Form,TimeLine_RenewPeriod"))
                .Distinct()
                .ToList();
        }

        public IPagedList<ContractorDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ContractorDTO>(filterModel);
            return dapperExecution.ExecutePaginateQuery();
        }

        public IPagedList<ContractorDTO> GetListByProjectIds(ContractorByProjectIdsFilterModel filterModel)
        {
            var cachedContractors = new Dictionary<int, ContractorDTO>();

            var dapperExecution = BuildByTemplate<ContractorDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.ContractCode");

            dapperExecution.SqlBuilder.Select("t3.StatusId");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContracts t2 ON t1.Id = t2.ContractorId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE ");// OutContractServicePackageStatus
            //dapperExecution.SqlBuilder.InnerJoin(
            //    "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE AND t3.StatusId NOT IN @outContractServicePackageStatus", new { outContractServicePackageStatus = new[] { OutContractServicePackageStatus.Terminate.Id, OutContractServicePackageStatus.ChangeServicePackage.Id } });// OutContractServicePackageStatus

            if (!string.IsNullOrEmpty(filterModel.ProjectIds))
            {
                var projectIds = filterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t2.ProjectId IN @projectIds Or t2.ProjectId IS NULL)", new { projectIds })
                    ;
            }

            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractorCode LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.ContractorFullName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.ContractorPhone LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });

            }

            return dapperExecution.ExecutePaginateQuery<ContractorDTO, OutContractDTO, int?>(
                    (contractor, outcontract, statusId) =>
                    {
                        if (!cachedContractors.TryGetValue(contractor.Id, out var result))
                        {
                            result = contractor;
                            cachedContractors.Add(contractor.Id, contractor);
                        }

                        if (outcontract != null && result.ContractCodes.All(c => c != outcontract.ContractCode))
                        {
                            if (statusId != 2 && statusId != 3)
                                result.ContractCodes.Add(outcontract.ContractCode);
                        }
                        return result;
                    }, "Id, StatusId");
            //return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS('', t1.`ContractorFullName`, ', SĐT: ', t1.`ContractorPhone`, ', Đ/c: ', t1.`ContractorAddress`)  AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Select("t1.`ContractorCode` AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            return dapperExecution.ExecuteQuery();
        }

        public int GetContractorCountByProjectId(int projectId)
        {
            return WithConnection((conn)
                => conn.ExecuteScalar<int>(
                    "SELECT COUNT(1), t1.Id FROM Contractors t1 " +
                    "INNER JOIN OutContracts t2 on t2.ContractorId = t1.Id " +
                    "WHERE t2.ProjectId = @pId AND t1.IsDeleted = FALSE AND t2.IsDeleted = FALSE " +
                    "GROUP BY t1.Id",
                    new
                    {
                        pId = projectId
                    }));
        }

        public int GetTotalNumber()
        {
            return WithConnection((conn) =>
                conn.ExecuteScalar<int>("SELECT COUNT(1) " +
                "FROM Contractors " +
                "WHERE IsDeleted = FALSE"));
        }

        public IEnumerable<ContractorDTO> GetAll()
        {
            var dapperExecution = BuildByTemplate<ContractorDTO>();
            return dapperExecution.ExecuteQuery();
        }

        public int GetLatestId()
        {
            return WithConnection<int>((conn) => conn.ExecuteScalar<int>(
                   "SELECT Id FROM Contractors ORDER BY Id DESC LIMIT 1"));
        }

        public IEnumerable<ContractorDTO> GetAllByIds(int[] ids)
        {
            var dapperExecution = BuildByTemplate<ContractorDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<ContractorDTO> GetFromId(int fromId)
        {
            var dapperExecution = BuildByTemplate<ContractorDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id > @fromId", new { fromId });
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ContractorDTO> GetListByMarketIdsProjectIds(ContractorByMarketAreaIdsProjectIdsFilterModel filterModel)
        {
            var cachedContractors = new Dictionary<int, ContractorDTO>();

            var dapperExecution = BuildByTemplate<ContractorDTO>();
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.ContractCode");

            dapperExecution.SqlBuilder.Select("t3.StatusId");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContracts t2 ON t1.Id = t2.ContractorId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE ");
            // OutContractServicePackageStatus
            //dapperExecution.SqlBuilder.InnerJoin(
            //    "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE AND t3.StatusId NOT IN @outContractServicePackageStatus", new { outContractServicePackageStatus = new[] { OutContractServicePackageStatus.Terminate.Id, OutContractServicePackageStatus.ChangeServicePackage.Id } });// OutContractServicePackageStatus

            dapperExecution.SqlBuilder.Where("(t1.IsBuyer = true)");
            if (!string.IsNullOrEmpty(filterModel.ProjectIds))
            {
                var projectIds = filterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t2.ProjectId IN @projectIds)", new { projectIds })
                    ;
            }
            if (!string.IsNullOrEmpty(filterModel.MarketAreaIds))
            {
                var marketIds = filterModel.MarketAreaIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t2.MarketAreaId IN @marketIds)", new { marketIds })
                    ;
            }

            return dapperExecution.ExecutePaginateQuery<ContractorDTO, OutContractDTO, int?>(
                    (contractor, outcontract, statusId) =>
                    {
                        if (!cachedContractors.TryGetValue(contractor.Id, out var result))
                        {
                            result = contractor;
                            cachedContractors.Add(contractor.Id, contractor);
                        }

                        if (outcontract != null && result.ContractCodes.All(c => c != outcontract.ContractCode))
                        {
                            if (statusId != 2 && statusId != 3)
                                result.ContractCodes.Add(outcontract.ContractCode);
                        }
                        return result;
                    }, "Id, StatusId");
        }

        public IPagedList<ContractorDTO> GetListByProjectIdsForApp(ContractorByProjectIdsFilterModel filterModel)
        {
            var cachedContractors = new Dictionary<int, ContractorDTO>();

            var dapperExecution = BuildByTemplate<ContractorDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.ContractCode");

            dapperExecution.SqlBuilder.Select("t3.StatusId");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutContracts t2 ON t1.Id = t2.ContractorId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.InnerJoin(
                "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE ");// OutContractServicePackageStatus
            //dapperExecution.SqlBuilder.InnerJoin(
            //    "OutContractServicePackages t3 ON t2.Id = t3.OutContractId AND t3.IsDeleted = FALSE AND t3.StatusId NOT IN @outContractServicePackageStatus", new { outContractServicePackageStatus = new[] { OutContractServicePackageStatus.Terminate.Id, OutContractServicePackageStatus.ChangeServicePackage.Id } });// OutContractServicePackageStatus

            if (!string.IsNullOrEmpty(filterModel.ProjectIds))
            {
                var projectIds = filterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t2.ProjectId IN @projectIds)", new { projectIds })
                    ;
            }
            if (!string.IsNullOrEmpty(filterModel.ServiceIds))
            {
                var serviceIds = filterModel.ServiceIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t3.ServiceId IN @serviceIds)", new { serviceIds = serviceIds });
            }

            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractorCode LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.ContractorFullName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.ContractorPhone LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });

            }

            return dapperExecution.ExecutePaginateQuery<ContractorDTO, OutContractDTO, int?>(
                    (contractor, outcontract, statusId) =>
                    {
                        if (!cachedContractors.TryGetValue(contractor.Id, out var result))
                        {
                            result = contractor;
                            cachedContractors.Add(contractor.Id, contractor);
                        }

                        if (outcontract != null && result.ContractCodes.All(c => c != outcontract.ContractCode))
                        {
                            if (statusId != 2 && statusId != 3)
                                result.ContractCodes.Add(outcontract.ContractCode);
                        }
                        return result;
                    }, "Id, StatusId");
        }
    }
}