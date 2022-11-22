using ApplicationUserIdentity.API.Application.Commands.CustomerCategory;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
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
    public class CustomerClassMappingConfigs : Profile
    {
        public CustomerClassMappingConfigs()
        {
            CreateMap<ApplicationUserClass, UserClassViewModel>().ReverseMap();

            CreateMap<CustomerClassModelGrpc, UserClassViewModel>().ReverseMap();
            CreateMap<IPagedList<UserClassViewModel>, CustomerClassPageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));

        }
    }
}
