using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.PackageModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class PackageMapperProfile : Profile
    {
        public PackageMapperProfile()
        {

            CreateMap<PackageFilterModel, PackageRequestGrpc>().ReverseMap();
            CreateMap<PackageDTO, PackageGrpcDTO>().ReverseMap();
            CreateMap(typeof(PackagePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
