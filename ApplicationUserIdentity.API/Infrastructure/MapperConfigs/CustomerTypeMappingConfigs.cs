using ApplicationUserIdentity.API.Application.Commands.CustomerType;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class CustomerTypeMappingConfigs : Profile
    {
        public CustomerTypeMappingConfigs()
        {
            CreateMap<CustomerTypeDTO, CustomerType>();
            CreateMap<CustomerType, CustomerStructureDTO>();
            CreateMap<CustomerTypeCommand, CustomerType>().ReverseMap();


            CreateMap<CustomerTypeModelGrpc, CustomerTypeDTO>().ReverseMap();
            CreateMap<IPagedList<CustomerTypeDTO>, CustomerTypePageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));
        }
    }
}
