using AutoMapper;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Models.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class UserRoleMapperConfigs : Profile
    {
        public UserRoleMapperConfigs()
        {
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
        }
    }
}
