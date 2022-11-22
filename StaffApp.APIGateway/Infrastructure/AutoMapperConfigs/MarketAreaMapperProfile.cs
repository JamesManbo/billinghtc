using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.MarketAreaModels;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class MarketAreaMapperProfile : Profile
    {
        public MarketAreaMapperProfile()
        {

            CreateMap<MarketAreaModelDTO, MarketAreaGrpcDTO>().ReverseMap();

           // CreateMap(typeof(TaxCategoryPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            CreateMap<MarketAreaPageListGrpcDTO, IPagedList<MarketAreaModelDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
