using AutoMapper;
using OrganizationUnit.API.Application.Commands;
using OrganizationUnit.API.Models;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.Models.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class PermissionMapperConfigs : Profile
    {
        public PermissionMapperConfigs()
        {
            CreateMap<Permission, CreatePermissionCommand>().ReverseMap();
            CreateMap<Permission, UpdatePermissionCommand>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<CreatePermissionCommand, PermissionDTO>().ReverseMap();
            CreateMap<PermissionDTO, UpdatePermissionCommand>().ReverseMap();
            CreateMap<PermissionDTO, Permission>().ReverseMap();
        }
    }
}
