using AutoMapper;
using ContractManagement.Domain.AggregatesModel.MarketArea;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Commands.MarketAreaCommand;
using Global.Models.PagedList;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class MarketAreaMapperConfigs : Profile
    {
        public MarketAreaMapperConfigs()
        {
            CreateMap<MarketArea, MarketAreaDTO>().ReverseMap();
            CreateMap<MarketArea, CUMarketAreaCommand>().ReverseMap();
            CreateMap<MarketAreaDTO, CUMarketAreaCommand>().ReverseMap();

            CreateMap<MarketAreaDTO, MarketAreaGrpcDTO>();
            CreateMap<MarketAreaDTO, MarketAreaGrpcDTO>().ReverseMap();
            CreateMap<IPagedList<MarketAreaDTO>, MarketAreaPageListGrpcDTO>()
                .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));

        }
    }
}
