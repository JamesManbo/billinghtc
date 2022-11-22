using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface IProjectGrpcService
    {
        Task<ProjectListGrpcDTO> GetAll();
    }
    public class ProjectGrpcService : GrpcCaller, IProjectGrpcService
    {
        public ProjectGrpcService(IMapper mapper, ILogger<ProjectGrpcService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ProjectListGrpcDTO> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new ProjectServiceGrpc.ProjectServiceGrpcClient(channel);
                return await client.GetAllAsync(new Empty());
            });
        }
    }
}
