using ApplicationUserIdentity.API.Application.Commands.CustomerCategory;
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
    public class CustomerCategoryMappingConfigs : Profile
    {
        public CustomerCategoryMappingConfigs()
        {
            CreateMap<CustomerCategoryDTO, CustomerCategory>().ReverseMap();
            CreateMap<CustomerCategory, CustomerCategoryDTO>().ReverseMap();
            CreateMap<CustomerCategoryCommand, CustomerCategory>().ReverseMap();

            CreateMap<CustomerCategoryModelGrpc, CustomerCategoryDTO>().ReverseMap();
            CreateMap<IPagedList<CustomerCategoryDTO>, CustomerCategoryPageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));

        }
    }
}
