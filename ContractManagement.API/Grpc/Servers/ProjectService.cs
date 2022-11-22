using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ContractManagement.API.Grpc.Servers
{
    public class ProjectService : ProjectServiceGrpc.ProjectServiceGrpcBase
    {
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IProjectQueries _projectQueries;
        private readonly IMapper _mapper;

        public ProjectService(IWrappedConfigAndMapper configAndMapper, IProjectQueries projectQueries, IMapper mapper)
        {
            _configAndMapper = configAndMapper;
            _projectQueries = projectQueries;
            _mapper = mapper;
        }

        public override Task<ProjectListGrpcDTO> GetAll(Empty request, ServerCallContext context)
        {
            var result =  new ProjectListGrpcDTO();
            var projects = _projectQueries.GetAll()?.ToList();

            if (projects != null && projects.Any())
            {
                for (int i = 0; i < projects.Count; i++)
                {
                    result.ProjectDtos.Add(projects.ElementAt(i).MapTo<ProjectGrpcDTO>(_configAndMapper.MapperConfig));
                }
            }

            return Task.FromResult(result);
        }

        public override Task<ProjectPageListGrpcDTO> GetProjects(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = _projectQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            var rs = _mapper.Map<ProjectPageListGrpcDTO>(lstProject);
            return Task.FromResult(rs);
        }

        public override Task<ProjectPageListGrpcDTO> GetProjectsForApp(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = _projectQueries.GetListForApp(_mapper.Map<RequestFilterModel>(request));
            var rs = _mapper.Map<ProjectPageListGrpcDTO>(lstProject);
            return Task.FromResult(rs);
        }

        public override Task<GetOutContractNumberResponseGrpc> GetOutContactCount(Int32Value request, ServerCallContext context)
        {
            var rs = _projectQueries.GetOutContactCount(request.Value);
            return Task.FromResult(new GetOutContractNumberResponseGrpc {Result = rs });
        }

        public override Task<StringValue> GetProjectCode(Int32Value request, ServerCallContext context)
        {
            var projectCode = _projectQueries.GetProjectCode(request.Value);
            return Task.FromResult(new StringValue() { Value = projectCode ?? string.Empty });
        }

        public override Task<ProjectListGrpcDTO> GetProjectOfSupporter(StringValue request, ServerCallContext context)
        {
            var projects = _projectQueries.GetProjectsOfSupporter(request.Value);
            return Task.FromResult(_mapper.Map<ProjectListGrpcDTO>(projects));
        }
    }
}
