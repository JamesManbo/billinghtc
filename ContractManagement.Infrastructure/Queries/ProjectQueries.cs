using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Text;
using Global.Models.Response;
using Dapper;
using AutoMapper;
using System.Linq;
using CachingLayer.Interceptor;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IProjectQueries : IQueryRepository
    {
        IPagedList<ProjectDTO> GetList(RequestFilterModel filterModel);
        IPagedList<ProjectDTO> GetListForApp(RequestFilterModel filterModel);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList();
        [Cache]
        IEnumerable<ProjectDTO> GetAll();
        IEnumerable<ProjectDTO> GetProjectsOfSupporter(string supporterId);
        ProjectDTO Find(int id);
        ProjectDTO GetById(int id);
        int GetOutContactCount(int projectId);
        string GetProjectCode(int projectId);
        List<int> GetAvaliableSupporterInProjectByProjectId(int projectId);
        List<string> GetAvaliableSupporterByOutContractId(int? outContractId = null, int? projectId = null);
        ProjectDTO FindByOutContractId(int outContractId);
        List<string> GetAvaliableSupporterById(int id);
    }
    public class ProjectQueries : QueryRepository<Project, int>, IProjectQueries
    {
        private readonly IMapper _mapper;

        public ProjectQueries(ContractDbContext contractDbContext, IMapper mapper) : base(contractDbContext)
        {
            _mapper = mapper;
        }

        public virtual IPagedList<ProjectDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.MarketAreaId = t2.Id");
            dapperExecution.SqlBuilder.Select("t2.MarketName as MarketName");
            dapperExecution.SqlBuilder.LeftJoin("ManagementBusinessBlocks t3 ON t1.BusinessBlockId = t3.Id");
            dapperExecution.SqlBuilder.Select("t3.BusinessBlockName as BusinessBlockName");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ProjectName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }
            return dapperExecution.ExecutePaginateQuery();
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.ProjectName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            dapperExecution.SqlBuilder.Select("t1.IdentityGuid AS GlobalValue");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<ProjectDTO> GetAll()
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>();
            return dapperExecution.ExecuteQuery();
        }

        public ProjectDTO Find(int id)
        {
            ProjectDTO result = null;
            var dapperExecution = BuildByTemplate<ProjectDTO>();
            dapperExecution.SqlBuilder.Select("pt.UserTechnicianId");
            dapperExecution.SqlBuilder.LeftJoin("ProjectTechnicians AS pt ON t1.Id = pt.ProjectId");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            dapperExecution.ExecuteQuery<ProjectDTO, string>((project, technicianId) =>
            {
                if (result == null) result = project;

                if (string.IsNullOrEmpty(technicianId))
                {
                    result.Technicians.Add(technicianId);
                }
                return default;
            }, "Id,UserTechnicianId");
            return result;
        }

        public IPagedList<ProjectDTO> GetListForApp(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("MarketAreas t2 ON t1.MarketAreaId = t2.Id AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("OutContracts t3 ON t3.ProjectId = t1.Id AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Select("t2.MarketName as MarketName");
            dapperExecution.SqlBuilder.Select("count(t3.id) as NumberOfOutContracts");
            dapperExecution.SqlBuilder.GroupBy("t1.id");

            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ProjectName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.ProjectCode LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }
            return dapperExecution.ExecutePaginateQuery();
        }

        public int GetOutContactCount(int projectId)
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>();
            dapperExecution.SqlBuilder.LeftJoin("OutContracts t3 ON t3.ProjectId = t1.Id AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Select("count(t3.id) as NumberOfOutContracts");

            dapperExecution.SqlBuilder.Where("t3.ProjectId = @projId", new { projId = projectId });
            var rs = dapperExecution.ExecuteScalarQuery();
            return rs.NumberOfOutContracts ?? 0;
        }

        public string GetProjectCode(int projectId)
        {
            return WithConnection(conn =>
                conn.QueryFirstOrDefault<string>(
                    "SELECT ProjectCode FROM Projects WHERE Id = @projectId AND IsDeleted = FALSE", new { projectId = projectId }));
        }

        public ProjectDTO GetById(int id)
        {
            ProjectDTO result = new ProjectDTO();
            var dapperExecution = BuildByTemplate<ProjectRaw>();
            dapperExecution.SqlBuilder.Select("pt.UserTechnicianId");
            dapperExecution.SqlBuilder.LeftJoin("ProjectTechnicians AS pt ON t1.Id = pt.ProjectId");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            var masterData = dapperExecution.ExecuteQuery();
            _mapper.Map(masterData.FirstOrDefault(), result);
            foreach (var data in masterData)
            {
                if (!string.IsNullOrEmpty(data.UserTechnicianId))
                    result.Technicians.Add(data.UserTechnicianId);
            }
            return result;
        }

        public IEnumerable<ProjectDTO> GetProjectsOfSupporter(string supporterId)
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>();
            dapperExecution.SqlBuilder.InnerJoin("ProjectTechnicians pt ON pt.ProjectId = t1.Id AND pt.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("pt.UserTechnicianId = @supporterId", new { supporterId });
            return dapperExecution.ExecuteQuery();
        }
        public List<int> GetAvaliableSupporterInProjectByProjectId(int projectId)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<int>();
            dapperExecution.SqlBuilder.Select("t2.UserTechnicianId AS Id");
            dapperExecution.SqlBuilder.RightJoin("ProjectTechnicians AS t2 ON t1.Id = t2.ProjectId");
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            dapperExecution.SqlBuilder.Where("t2.ProjectId = @projectId", new { projectId });

            return dapperExecution.ExecuteQuery().ToList();
        }

        public List<string> GetAvaliableSupporterByOutContractId(int? outContractId = null, int? projectId = null)
        {
            if (!outContractId.HasValue && !projectId.HasValue) return new List<string>();

            var dapperExecution = BuildByTemplateWithoutSelect<string>();
            dapperExecution.SqlBuilder.Select("t2.UserTechnicianId");
            dapperExecution.SqlBuilder.RightJoin("ProjectTechnicians AS t2 ON t1.Id = t2.ProjectId");
            if (outContractId.HasValue)
            {
                dapperExecution.SqlBuilder.InnerJoin("OutContracts AS t3 ON t1.Id = t3.ProjectId AND t3.Id = @outContractId", new { outContractId });
            }
            else if (projectId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @projectId", new { projectId });
            }
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");

            return dapperExecution.ExecuteQuery().ToList();
        }
        public List<string> GetAvaliableSupporterById(int id)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<string>();
            dapperExecution.SqlBuilder.Select("t2.UserTechnicianId");
            dapperExecution.SqlBuilder.RightJoin("ProjectTechnicians AS t2 ON t1.Id = t2.ProjectId");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteQuery().ToList();
        }

        public ProjectDTO FindByOutContractId(int outContractId)
        {
            var dapperExecution = BuildByTemplate<ProjectDTO>();
            dapperExecution.SqlBuilder.LeftJoin("OutContracts AS oc ON oc.Id = @outContractId AND oc.ProjectId = t1.Id", new { outContractId });

            return dapperExecution.ExecuteScalarQuery();
        }
    }
}
