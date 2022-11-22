using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.Filter;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ServicesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            CreateMap<ServiceDTO, ServiceGrpcDTO>().ReverseMap();
            CreateMap<PictureDTOGrpc, PictureViewDTO>().ReverseMap();

            CreateMap(typeof(ServicePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            //CreateMap<ServicePageListGrpcDTO, IPagedList<ServiceDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }

    }
}
