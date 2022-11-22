using AutoMapper;
using CustomerApp.APIGateway.Models.SupportLocationModel;
using Global.Models.Filter;
using Global.Models.PagedList;
using Location.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class SupportLocationMapperProfile : Profile
    {
        public SupportLocationMapperProfile()
        {
            CreateMap<RequestFilterModel, Location.API.Protos.RequestFilterGrpc>().ReverseMap();
            CreateMap<SupportLocationDTO, SupportLocationGrpcDTO>().ReverseMap();

            // CreateMap(typeof(TaxCategoryPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            CreateMap<SupportLocationPageListGrpcDTO, IPagedList<SupportLocationDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
