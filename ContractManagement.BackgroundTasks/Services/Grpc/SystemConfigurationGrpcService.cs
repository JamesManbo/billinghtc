using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Configs.SystemArgument;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BackgroundTasks.Services.Grpc
{
    public interface ISystemConfigurationGrpcService
    {
        Task<SystemConfigurationParamsGrpcDTO> GetSystemConfigurations();
    }
    public class SystemConfigurationGrpcService : GrpcCaller, ISystemConfigurationGrpcService
    {
        public SystemConfigurationGrpcService(IMapper mapper,
            ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
        }

        public async Task<SystemConfigurationParamsGrpcDTO> GetSystemConfigurations()
        {
            return await CallAsync(async channel =>
            {
                var client = new ConfigurationSystemParameterGrpc.ConfigurationSystemParameterGrpcClient(channel);
                var grpcResponse = await client.GetSystemConfigurationAsync(new Empty());
                return grpcResponse;
            });
        }
    }
}
