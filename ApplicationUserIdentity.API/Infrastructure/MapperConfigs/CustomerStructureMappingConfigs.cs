using ApplicationUserIdentity.API.Application.Commands.CustomerStructure;
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
    public class CustomerStructureMappingConfigs : Profile
    {
        public CustomerStructureMappingConfigs()
        {
            CreateMap<CustomerStructureDTO, CustomerStructure>();
            CreateMap<CustomerStructure,CustomerStructureDTO>();
            CreateMap<CustomerStructureCommand, CustomerStructure>().ReverseMap();

            CreateMap<CustomerStructureDTO, CustomerStructModelGrpc>().ReverseMap();
            CreateMap<IPagedList<CustomerStructureDTO>, CustomerStructPageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));
        }
    }
}
