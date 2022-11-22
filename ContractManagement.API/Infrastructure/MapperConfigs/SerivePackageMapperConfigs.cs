using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using Global.Models.PagedList;
using Global.Models.Filter;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Commands.ServiceCommand;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class SerivePackageMapperConfigs : Profile
    {
        public SerivePackageMapperConfigs()
        {
            CreateMap<ServiceGroup, ServiceGroupDTO>().ReverseMap();

            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceCommand, Service>();

            CreateMap<ServicePackage, ServicePackageDTO>();
            CreateMap<ServicePackage, CUServicePackageCommand>().ReverseMap();
            CreateMap<ServicePackagePrice, ServicePackagePriceDTO>();
            CreateMap<PictureDTO, PictureDTOGrpc>().ReverseMap();

            CreateMap<ServicePackageRadiusServiceCommand, ServicePackageRadiusService>();
            CreateMap<ServicePackageRadiusService, ServicePackageRadiusServiceDTO>();

            CreateMap<ServiceDTO, ServiceGrpcDTO>().ReverseMap();
            CreateMap<IPagedList<ServiceDTO>, ServicePageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

            CreateMap<ServicePackageDTO, PackageGrpcDTO>();
            CreateMap<ServicePackageSimpleDTO, PackageGrpcDTO>();
            CreateMap<PackageFilterModel, PackageRequestGrpc>().ReverseMap();
            CreateMap<IPagedList<ServicePackageDTO>, PackagePageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }        
    }
}
