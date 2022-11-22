using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IProjectService
    {
        Task<IPagedList<ProjectDTO>> GetList(RequestFilterModel filterModel);
        Task<int> GetOutContactCount(int projectId);
    }
    public class ProjectService : GrpcCaller, IProjectService
    {
        private readonly IMapper _mapper;
        public ProjectService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ProjectDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<ProjectDTO>>(async channel =>
            {
                var client = new ProjectServiceGrpc.ProjectServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstProjectGrpc = await client.GetProjectsAsync(request);

                return _mapper.Map<IPagedList<ProjectDTO>>(lstProjectGrpc);
            });
        }

        public async Task<int> GetOutContactCount(int projectId)
        {
            return await Call<int>(async channel =>
            {
                var client = new ProjectServiceGrpc.ProjectServiceGrpcClient(channel);

                var rs = await client.GetOutContactCountAsync(new Int32Value { Value = projectId});

                return rs.Result;
            });
        }
    }
}
