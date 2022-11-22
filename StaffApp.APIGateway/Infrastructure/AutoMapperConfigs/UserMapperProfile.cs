using AutoMapper;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Users;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.ProjectModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {

            CreateMap<UserFilterModel, UserRequestFilterModelGrpc>().ReverseMap();
            CreateMap<UserDTO, UserDtoGrpc>().ReverseMap();
            CreateMap<ProjectDTO, ProjectDtoGrpc>().ReverseMap();
            CreateMap<ConfigurationUserDto, ConfigurationUserDtoGrpc>().ReverseMap();

        }
    }
}
