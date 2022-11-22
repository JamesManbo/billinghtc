using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.TaxCategoryModels;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class TaxCategoryMapperProfile : Profile
    {
        public TaxCategoryMapperProfile()
        {

            CreateMap<TaxCategoryDTO, TaxCategoryGrpcDTO>().ReverseMap();

           // CreateMap(typeof(TaxCategoryPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            CreateMap<TaxCategoryPageListGrpcDTO, IPagedList<TaxCategoryDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
