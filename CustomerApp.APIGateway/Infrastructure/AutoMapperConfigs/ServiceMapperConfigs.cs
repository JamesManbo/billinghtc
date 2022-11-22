using AutoMapper;
using CustomerApp.APIGateway.Models;
using ContractManagement.API.Protos;
using Global.Models.PagedList;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ServiceMapperConfigs : Profile
    {
        public ServiceMapperConfigs()
        {
            CreateMap<ServiceDTO, ServiceGrpcDTO>().ReverseMap(); 
            CreateMap<ServiceListDTO, ServiceListGrpcDTO>().ReverseMap();
            CreateMap(typeof(ServicePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
