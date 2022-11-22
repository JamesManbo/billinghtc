using AutoMapper;
using OrganizationUnit.API.Application.Commands.Role;
using OrganizationUnit.API.Models;
using OrganizationUnit.Domain.Models.Role;
using OrganizationUnit.Domain.RoleAggregate;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class RoleMapperConfigs : Profile
    {
        public RoleMapperConfigs()
        {
            CreateMap<Role, CreateRoleCommand>().ReverseMap();
            CreateMap<Role, UpdateRoleCommand>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<RoleDTO, CreateRoleCommand>().ReverseMap();
            CreateMap<RoleDTO, UpdateRoleCommand>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}
