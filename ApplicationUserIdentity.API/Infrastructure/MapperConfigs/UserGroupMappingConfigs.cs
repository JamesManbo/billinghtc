using ApplicationUserIdentity.API.Application.Commands.UserGroup;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class UserGroupMappingConfigs : Profile
    {
        public UserGroupMappingConfigs()
        {
            CreateMap<ApplicationUserGroup, UserGroupDTO>().ReverseMap();
            CreateMap<CUUserGroupCommand, UserGroupDTO>().ReverseMap();
            CreateMap<CUUserGroupCommand, ApplicationUserGroup>().ReverseMap();


            CreateMap<CustomerGroupModelGrpc, UserGroupDTO>().ReverseMap();
            CreateMap<IPagedList<UserGroupDTO>, CustomerGroupPageListGrpc>()
               .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));
        }
    }
}
