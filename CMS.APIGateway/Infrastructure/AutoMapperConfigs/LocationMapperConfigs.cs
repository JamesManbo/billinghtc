using AutoMapper;
using CMS.APIGateway.Models;
using Location.API.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class LocationMapperConfigs : Profile
    {
        public LocationMapperConfigs()
        {
            CreateMap<CreateDraftLocationCommand, CreateLocationGrpcCommand>().ReverseMap();
            CreateMap<LocationDTO, LocationGrpcDTO>().ReverseMap();
            
            CreateMap<CreateDraftMarketAreaCommand, CreateMarketAreaGrpcCommand>().ReverseMap();
            CreateMap<MarketAreaDTO, MarketAreaGrpcDTO>().ReverseMap();

        }
    }
}
