using ContractManagement.API.Protos;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IProjectGrpcService
    {
        Task<string> GetProjectCode(int id);
    }
    public class ProjectGrpcService : GrpcCaller, IProjectGrpcService
    {
        public ProjectGrpcService(ILogger<GrpcCaller> logger) 
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<string> GetProjectCode(int id)
        {
            return await CallAsync(async channel =>
            {
                var client = new ProjectServiceGrpc.ProjectServiceGrpcClient(channel);
                var result = await client.GetProjectCodeAsync(new Int32Value { Value = id });
                return result.Value;
            });
        }
    }
}
