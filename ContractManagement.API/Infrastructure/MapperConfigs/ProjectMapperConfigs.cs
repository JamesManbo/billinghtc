using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models;
using System.Collections.Generic;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Commands.ProjectCommand;
using Global.Models.PagedList;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ProjectMapperConfigs : Profile
    {
        public ProjectMapperConfigs()
        {
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Project, CUProjectCommand>().ReverseMap();
            CreateMap<CUProjectCommand, ProjectDTO>().ReverseMap();

            CreateMap<ProjectDTO, ProjectGrpcDTO>();
            CreateMap<IPagedList<ProjectDTO>, ProjectPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

            CreateMap<ProjectDTO, ProjectRaw>().ReverseMap(); ;
            CreateMap<List<ProjectDTO>, ProjectListGrpcDTO>().ForMember(s => s.ProjectDtos, d => d.MapFrom(e => e));
        }
    }
}
