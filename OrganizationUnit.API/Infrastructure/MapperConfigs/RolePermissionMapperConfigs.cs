using AutoMapper;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.Models.RolePermission;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class RolePermissionMapperConfigs : Profile
    {
        public RolePermissionMapperConfigs()
        {
            CreateMap<RolePermission, RolePermissionDto>().ReverseMap();
        }
    }
}
