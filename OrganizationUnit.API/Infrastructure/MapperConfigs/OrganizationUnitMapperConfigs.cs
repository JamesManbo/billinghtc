using AutoMapper;
using OrganizationUnit.API.Application.Commands.OrganizationUnit;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.API.Protos;
using OrganizationUnit.API.Protos.Organizations;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class OrganizationUnitMapperConfigs : Profile
    {
        public OrganizationUnitMapperConfigs()
        {
            CreateMap<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, CreateOrganizationUnitCommand>().ReverseMap();
            CreateMap<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, UpdateOrganizationUnitCommand>().ReverseMap();
            CreateMap<Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit, OrganizationUnitDTO>().ReverseMap();
            CreateMap<CreateOrganizationUnitCommand, OrganizationUnitDTO>().ReverseMap();
            CreateMap<OrganizationUnitDTO, UpdateOrganizationUnitCommand>().ReverseMap();
            CreateMap<OrganizationUnitDTO, Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit>().ReverseMap();
            CreateMap<OrganizationUnitDTO, OrganizationUnitGrpcDTO>();
        }
    }
}
