using AutoMapper;
using ContractManagement.Domain.Models.Organizations;
using OrganizationUnit.API.Protos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class OrganizationMapperConfig : Profile
    {
        public OrganizationMapperConfig()
        {
            CreateMap<UserDtoGrpc, UserDTO>();
        }
    }
}
