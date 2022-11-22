using AutoMapper;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IConfigurationSystemParameterGrpcService
    {
        Task<SystemConfigurationParamsGrpcDTO> GetSystemConfigurations();
    }

    public class ConfigurationSystemParameterGrpcService : GrpcCaller, IConfigurationSystemParameterGrpcService
    {
        public ConfigurationSystemParameterGrpcService(ILogger<GrpcCaller> logger)
            : base(logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
        }

        public async Task<SystemConfigurationParamsGrpcDTO> GetSystemConfigurations()
        {
            return await CallAsync(async channel =>
            {
                var client = new ConfigurationSystemParameterGrpc.ConfigurationSystemParameterGrpcClient(channel);
                var numberDaysBadDebt = await client.GetSystemConfigurationAsync(new Empty());
                return numberDaysBadDebt;
            });
        }
    }
}
