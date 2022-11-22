using ApplicationUserIdentity.API.Models.LocationDomainModels;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Location.API.Proto;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Grpc.Clients.Location
{
    public interface ILocationGrpcService
    {
        Task<LocationDTO> GetByLocationId(string id);
    }

    public class LocationGrpcService : GrpcCaller, ILocationGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        public LocationGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger) : base(logger, MicroserviceRouterConfig.GrpcLocation)
        {
            _wrappedConfig = wrappedConfig;
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
    }
}
