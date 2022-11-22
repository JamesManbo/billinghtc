using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.ServicePackageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class PackageMapperProfile : Profile
    {
        public PackageMapperProfile()
        {

            CreateMap<ServicePackageDTO, PackageGrpcDTO>().ReverseMap();
            CreateMap<PackageRequestGrpc, PackageFilterModel>().ReverseMap();
            CreateMap(typeof(PackagePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            //CreateMap<ServicePageListGrpcDTO, IPagedList<ServiceDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
