using ContractManagement.Domain.Models.Location;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Location.API.Proto;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Clients.Location
{
    public interface ILocationGrpcService
    {
        Task<List<LocationDTO>> GetListByLevel(int level);
        Task<LocationDTO> GetByLocationId(string id);
        Task<LocationDTO> GetByLocationCode(string code);
    }

    public class LocationGrpcService : GrpcCaller, ILocationGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        public LocationGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger) : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcLocation)
        {
            _wrappedConfig = wrappedConfig;
        }

        public async Task<List<LocationDTO>> GetListByLevel(int level)
        {
            return await CallAsync(async channel =>
            {
                var client = new LocationGrpc.LocationGrpcClient(channel);
                var resultGrpc = await client.GetListByLevelAsync(new Int32Value { Value = level });
                var locations = resultGrpc.LocationGrpcDTOs.MapTo<List<LocationDTO>>(_wrappedConfig.MapperConfig);
                return locations;
            });
        }

        public async Task<LocationDTO> GetByLocationId(string id)
        {
            return await CallAsync(async channel =>
            {
                var client = new LocationGrpc.LocationGrpcClient(channel);
                var resultGrpc = await client.GetByLocationIdAsync(new StringValue { Value = id });
                var location = resultGrpc.MapTo<LocationDTO>(_wrappedConfig.MapperConfig);
                return location;
            });
        }

        public async Task<LocationDTO> GetByLocationCode(string code)
        {
            return await CallAsync(async channel =>
            {
                var client = new LocationGrpc.LocationGrpcClient(channel);
                var resultGrpc = await client.GetByLocationCodeAsync(new StringValue { Value = code });
                var location = resultGrpc.MapTo<LocationDTO>(_wrappedConfig.MapperConfig);
                return location;
            });
        }
    }
}
