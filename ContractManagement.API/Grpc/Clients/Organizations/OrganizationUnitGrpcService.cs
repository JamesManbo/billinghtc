using ContractManagement.Domain.Models.Organizations;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Clients.Organizations
{
    public interface IOrganizationUnitGrpcService
    {
        Task<IEnumerable<OrganizationUnitDTO>> GetAll();
        Task<IEnumerable<OrganizationUnitDTO>> GetChildrenByCode(string parentCode);
    }
    public class OrganizationUnitGrpcService : GrpcCaller, IOrganizationUnitGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;

        public OrganizationUnitGrpcService(IWrappedConfigAndMapper wrappedConfig,
            ILogger<GrpcCaller> logger) : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcOrganizationUnit)
        {
            this._wrappedConfig = wrappedConfig;
        }

        public async Task<IEnumerable<OrganizationUnitDTO>> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new OrganizationUnitsGrpc.OrganizationUnitsGrpcClient(channel);
                var grpcResponse = await client.GetAllAsync(new Empty());
                if (grpcResponse == null || grpcResponse.OrganizationUnits == null || grpcResponse.OrganizationUnits.Count == 0)
                {
                    return default;
                }

                return grpcResponse.OrganizationUnits
                    .Select(o => o.MapTo<OrganizationUnitDTO>(this._wrappedConfig.MapperConfig));
            });
        }

        public async Task<IEnumerable<OrganizationUnitDTO>> GetChildrenByCode(string parentCode)
        {
            return await CallAsync(async channel =>
            {
                var client = new OrganizationUnitsGrpc.OrganizationUnitsGrpcClient(channel);
                var grpcResponse = await client.GetChildrenByCodeAsync(new StringValue()
                {
                    Value = parentCode
                });
                if (grpcResponse == null || grpcResponse.OrganizationUnits == null || grpcResponse.OrganizationUnits.Count == 0)
                {
                    return default;
                }

                return grpcResponse.OrganizationUnits
                    .Select(o => o.MapTo<OrganizationUnitDTO>(this._wrappedConfig.MapperConfig));
            });
        }
    }
}
