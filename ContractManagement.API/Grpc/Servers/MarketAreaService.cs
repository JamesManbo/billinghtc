using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ContractManagement.API.Grpc.Servers
{
    public class MarketAreaService : MarketAreaServiceGrpc.MarketAreaServiceGrpcBase
    {
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IMapper _mapper;

        public MarketAreaService(IWrappedConfigAndMapper configAndMapper, IMarketAreaQueries marketAreaQueries, IMapper mapper)
        {
            _configAndMapper = configAndMapper;
            _marketAreaQueries = marketAreaQueries;
            _mapper = mapper;
        }

        public override Task<MarketAreaListGrpcDTO> GetAll(Empty request, ServerCallContext context)
        {
            var result = new MarketAreaListGrpcDTO();
            var marketAreas = _marketAreaQueries.GetAll().ToList();

            for (int i = 0; i < marketAreas.Count; i++)
            {
                result.MarketAreaDtos.Add(marketAreas.ElementAt(i).MapTo<MarketAreaGrpcDTO>(_configAndMapper.MapperConfig));
            }

            return Task.FromResult(result);
        }

        public override Task<MarketAreaPageListGrpcDTO> GetMarketArea(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = _marketAreaQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            return Task.FromResult(_mapper.Map<MarketAreaPageListGrpcDTO>(lstProject));
        }

        public override Task<StringValue> GetMarketAreaCode(Int32Value request, ServerCallContext context)
        {
            var marketAreaCode = _marketAreaQueries.GetMarketAreaCode(request.Value);
            return Task.FromResult(new StringValue() { Value = marketAreaCode ?? string.Empty });
        }
    }
}
