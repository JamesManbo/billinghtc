using AutoMapper;
using ContractManagement.API.Protos;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Models;
using SystemUserIdentity.API.Proto;

namespace SystemUserIdentity.API.Infrastructure.AutoMapperConfigs
{
    public class AuthenticationMapperConfigs : Profile
    {
        public AuthenticationMapperConfigs()
        {
            CreateMap<ChangePasswordResultGrpcDTO, UserIdentityResult>().ReverseMap();
            CreateMap<UserError, UserIdentityError>().ReverseMap();
            CreateMap<SignInResult, SignInResultGrpc>().ReverseMap();
            CreateMap<OrganizationUnit.Domain.Models.User.UserDTO, UserGrpcDTO>()
                .ForMember(s => s.ProjectIds, d => d.MapFrom(e => e.ProjectIds));
            CreateMap<ProjectDtoGrpc,OrganizationUnit.Domain.Models.User.ProjectDTO>().ReverseMap();
            CreateMap<ProjectGrpcDTO, OrganizationUnit.Domain.Models.User.ProjectDTO>().ReverseMap();
            CreateMap<OrganizationUnit.Domain.Models.Common.PictureDto, PictureGrpcDto>().ReverseMap();

            CreateMap<FCMToken, RegisterFCMTokenCommandGrpc>().ReverseMap();

            CreateMap<ConfigurationSettingUserDto, ConfigurationUserDtoGrpc>().ReverseMap();
        }
    }
}
