using Dapper;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using System;
using System.Collections.Generic;
using System.Text;
using Global.Models.Response;
using System.Linq;
using CachingLayer.Interceptor;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IOrganizationUnitQueryRepository : IQueryRepository
    {
        IPagedList<OrganizationUnitDTO> GetList(RequestFilterModel filterModel);
        [Cache]
        IEnumerable<OrganizationUnitDTO> GetAll(RequestFilterModel filterModel = null);
        OrganizationUnitDTO GetByCode(string code);
        OrganizationUnitDTO GetById(int id);
        OrganizationUnitDTO GetByUniversalIdentity(string id);
        [Cache]
        IEnumerable<OrganizationUnitDTO> GetAllChildByCode(string code);
        IEnumerable<OrganizationUnitDTO> GetAllChildById(int organizationUnitId);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList();
        [Cache]
        IEnumerable<HierarchicalItem> GetHierarchicalList();
        [Cache]
        IEnumerable<SelectionItem> GetAutocompleteParents(string name);
    }

    public class OrganizationUnitQueryRepository : QueryRepository<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, int>, IOrganizationUnitQueryRepository
    {
        public OrganizationUnitQueryRepository(OrganizationUnitDbContext organizationUnitDbContext) : base(organizationUnitDbContext)
        {
        }

        public IPagedList<OrganizationUnitDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<OrganizationUnitDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnits t2 ON t2.`Id` = t1.`ParentId` AND t2.`IsDeleted` = 0");
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers AS oum ON oum.OrganizationUnitId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("Users t3 ON t3.Id = oum.UserId AND oum.PositionLevel = 1");
            dapperExecution.SqlBuilder.Select("t2.`Name` AS `ParentName`");

            dapperExecution.SqlBuilder.Select("GROUP_CONCAT(t3.FullName) AS ManagementUsers");

            //dapperExecution.SqlBuilder.LeftJoin("Locations t3 ON t3.`Id` = t1.`ProvinceId` AND t3.`IsDeleted` = 0");
            //dapperExecution.SqlBuilder.Select("t3.`Name` AS `ProvinceName`");
            //if (filterModel.Paging == false)
            //{
            //    dapperExecution.SqlBuilder.Select("(SELECT COUNT(1) FROM Users WHERE `OrganizationUnitId` = t1.`Id`) AS `TotalEmployees`");
            //}

            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.`Name` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            dapperExecution.SqlBuilder.Select("t1.`ParentId` AS `ParentId`");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<HierarchicalItem> GetHierarchicalList()
        {
            var dapperExecution = BuildByTemplate<HierarchicalItem>();
            dapperExecution.SqlBuilder.Select("t1.`Name` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            dapperExecution.SqlBuilder.Select("t1.`ParentId` AS `ParentId`");
            dapperExecution.SqlBuilder.Select("t1.`Code` AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.`TreePath` AS `TreePath`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            dapperExecution.SqlBuilder.Select("(CASE " +
                              "         WHEN EXISTS(" +
                              "             SELECT NULL AS `EMPTY`" +
                              "             FROM OrganizationUnits AS t2" +
                              "             WHERE t2.`ParentId` = t1.`Id` AND t2.`IsDeleted` = 0" +
                              "             ) THEN 1" +
                              "         ELSE 0" +
                              "     END) AS `HasChildren`");

            dapperExecution.SqlBuilder.Select("t3.FullName AS ManagementUserName");

            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers AS oum ON oum.OrganizationUnitId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("Users t3 ON t3.Id = oum.UserId AND oum.PositionLevel = 1");

            var cacheResult = new Dictionary<int, HierarchicalItem>();

            var query = dapperExecution.ExecuteQuery<HierarchicalItem, string>(
                (orgUnit, managementUser) =>
                {
                    if (!cacheResult.TryGetValue(orgUnit.Value, out var result))
                    {
                        result = orgUnit;
                        cacheResult.Add(result.Value, result);
                    }

                    if (!string.IsNullOrEmpty(managementUser))
                    {
                        if (string.IsNullOrEmpty(result.Description))
                        {
                            result.Description = managementUser;
                        }
                        else
                        {
                            result.Description += $", {managementUser}";
                        }
                    }

                    return result;
                },
                "ManagementUserName"
            );

            return query.Distinct();
        }

        public IEnumerable<SelectionItem> GetAutocompleteParents(string name)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.`Name` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            dapperExecution.SqlBuilder.Select("t1.`ParentId` AS `ParentId`");
            dapperExecution.SqlBuilder.Where($"t1.`Name` LIKE '%{name}%'");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<OrganizationUnitDTO> GetAll(RequestFilterModel filterModel = null)
        {
            var dapperExecution = BuildByTemplate<OrganizationUnitDTO>(filterModel);
            return dapperExecution.ExecuteQuery();
        }

        private OrganizationUnitDTO GetDetail(object key = null, string uId = null)
        {
            var dapperExecution = BuildByTemplate<OrganizationUnitDTO>();

            dapperExecution.SqlBuilder.Select("t3.Id AS ManagementUserId");
            dapperExecution.SqlBuilder.Select("t3.FullName AS ManagementUserName");

            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers AS oum ON oum.OrganizationUnitId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("Users t3 ON t3.Id = oum.UserId AND oum.PositionLevel = 1");

            if (key != null)
            {
                if (key is int id)
                {
                    dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
                }
                else if (key is string code)
                {
                    dapperExecution.SqlBuilder.Where("LOWER(t1.Code) = LOWER(@code)", new { code });
                }
            }
            else
            {
                dapperExecution.SqlBuilder.Where("t1.IdentityGuid = @uId", new { uId });
            }

            var cacheResult = new Dictionary<int, OrganizationUnitDTO>();
            var query = dapperExecution.ExecuteQuery<OrganizationUnitDTO, int?, string>(
                (orgUnit, managementUserId, mngUserName) =>
                {
                    if (!cacheResult.TryGetValue(orgUnit.Id, out var result))
                    {
                        result = orgUnit;
                        cacheResult.Add(result.Id, result);
                    }

                    if (managementUserId != null && result.ManagementUserIds.IndexOf(managementUserId.Value) < 0)
                    {
                        result.ManagementUserIds.Add(managementUserId.Value);
                    }

                    if (!string.IsNullOrEmpty(mngUserName))
                    {
                        if (string.IsNullOrEmpty(result.ManagementUsers))
                        {
                            result.ManagementUsers = mngUserName;
                        }
                        else
                        {
                            result.ManagementUsers += $", {mngUserName}";
                        }
                    }

                    return result;
                },
                "ManagementUserId,ManagementUserName"
            );

            return query.FirstOrDefault();
        }

        private string GetTreePath(object key)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<string>();
            dapperExecution.SqlBuilder.Select("t1.TreePath");
            if (key is int id)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else if (key is string code)
            {
                dapperExecution.SqlBuilder.Where("t1.Code = @code", new { code });
            }
            return dapperExecution.ExecuteScalarQuery();
        }

        public OrganizationUnitDTO GetByCode(string code)
        {
            return GetDetail(code);
        }

        public OrganizationUnitDTO GetById(int id)
        {
            return GetDetail(id);
        }

        public OrganizationUnitDTO GetByUniversalIdentity(string id)
        {
            return GetDetail(uId: id);
        }

        public IEnumerable<OrganizationUnitDTO> GetAllChildByCode(string code)
        {
            var parentTreePath = this.GetTreePath(code);
            if (string.IsNullOrEmpty(parentTreePath)) return Enumerable.Empty<OrganizationUnitDTO>();

            var response = WithConnection(conn =>
                conn.Query<OrganizationUnitDTO>("SELECT t1.Id," +
                "t1.Code," +
                "t1.Address," +
                "t1.NumberPhone," +
                "t1.ShortName," +
                "t1.TreePath" +
                " FROM OrganizationUnits t1" +
                " WHERE t1.IsDeleted = FALSE AND t1.TreePath LIKE @parentPath",
                new
                {
                    parentPath = $"{parentTreePath}%"
                }));
            return response;
        }

        public IEnumerable<OrganizationUnitDTO> GetAllChildById(int organizationUnitId)
        {
            var parentTreePath = this.GetTreePath(organizationUnitId);
            if (string.IsNullOrEmpty(parentTreePath)) return Enumerable.Empty<OrganizationUnitDTO>();

            var response = WithConnection(conn =>
                conn.Query<OrganizationUnitDTO>("SELECT t1.Id," +
                "t1.Code," +
                "t1.Address," +
                "t1.NumberPhone," +
                "t1.ShortName," +
                "t1.TreePath" +
                " FROM OrganizationUnits t1" +
                " WHERE t1.IsDeleted = FALSE AND t1.TreePath LIKE @parentPath",
                new
                {
                    parentPath = $"{parentTreePath}%"
                }));
            return response;
        }
    }
}
