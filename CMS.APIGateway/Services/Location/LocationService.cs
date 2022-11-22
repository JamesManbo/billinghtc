using AutoMapper;
using CMS.APIGateway.Configs;
using CMS.APIGateway.Models;
using Location.API.Proto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.MicroserviceRouterConfig;

namespace CMS.APIGateway.Services.Location
{
    public class LocationService : GrpcCaller, ILocationService
    {
        private readonly ILogger<LocationService> _logger;
        private readonly IMapper _mapper;
        public LocationService(IMapper mapper, ILogger<LocationService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcLocation)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<LocationDTO> CreateDraftLocation(CreateDraftLocationCommand command)
        {
            return await Call<LocationDTO>(async channel =>
            {
                var client = new LocationGrpc.LocationGrpcClient(channel);
                var clientRequest = _mapper.Map<CreateLocationGrpcCommand>(command);
                return await client.CreateDraftLocationAsync(clientRequest);
            });
        }

        public async Task<MarketAreaDTO> CreateDraftMarketArea(CreateDraftMarketAreaCommand command)
        {
            return await Call<MarketAreaDTO>(async channel =>
            {
                var client = new MarketAreaGrpc.MarketAreaGrpcClient(channel);
                var clientRequest = _mapper.Map<CreateMarketAreaGrpcCommand>(command);
                return await client.CreateDraftMarketAreaAsync(clientRequest);
            });
        }
    }
}
