using ContractManagement.API.Protos;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemUserIdentity.API.Grpc.ClientServices
{
    public interface IProjectGrpcService
    {
        Task<ProjectListGrpcDTO> GetProjectsBySupporter(string supporterId);
    }

    public class ProjectGrpcService : GrpcCaller, IProjectGrpcService
    {
        public ProjectGrpcService(IWrappedConfigAndMapper wrappedConfig,
            ILogger<GrpcCaller> logger) : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ProjectListGrpcDTO> GetProjectsBySupporter(string supporterId)
        {
            return await Call<ProjectListGrpcDTO>(async channel =>
            {
                var client = new ProjectServiceGrpc.ProjectServiceGrpcClient(channel);
                var result = await client.GetProjectOfSupporterAsync(new StringValue()
                {
                    Value = supporterId
                });

                return result;
            });
        }
    }
}
