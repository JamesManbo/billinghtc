using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface IConfigurationSystemParameterGrpcService
    {
        Task<int> GetNumberDaysBadDebt();
        Task<SystemConfigurationParamsGrpcDTO> GetSystemConfigurations();
    }

    public class ConfigurationSystemParameterGrpcService: GrpcCaller, IConfigurationSystemParameterGrpcService
    {
        public ConfigurationSystemParameterGrpcService(IMapper mapper, ILogger<GrpcCaller> logger)
            : base(mapper, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
        }

        public async Task<int> GetNumberDaysBadDebt()
        {
            return await CallAsync<int>(async channel =>
            {
                var client = new ConfigurationSystemParameterGrpc.ConfigurationSystemParameterGrpcClient(channel);
                var numberDaysBadDebt = await client.GetNumberDaysBadDebtAsync(new Empty());
                return numberDaysBadDebt.Value;
            });
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
