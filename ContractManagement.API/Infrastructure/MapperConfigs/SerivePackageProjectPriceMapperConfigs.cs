using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class SerivePackageProjectPriceMapperConfigs : Profile
    {
        public SerivePackageProjectPriceMapperConfigs()
        {            
            CreateMap<ServicePackagePrice, ServicePackagePriceDTO>().ReverseMap();
            CreateMap<ServicePackagePrice, CUServicePackagePriceCommand>().ReverseMap();
            CreateMap<ServicePackagePriceDTO, CUServicePackagePriceCommand>().ReverseMap();
        }
    }
}
