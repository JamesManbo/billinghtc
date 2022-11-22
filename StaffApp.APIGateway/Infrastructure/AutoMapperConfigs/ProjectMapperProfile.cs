using AutoMapper;
using ContractManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ProjectMapperProfile : Profile
    {
        public ProjectMapperProfile()
        {

            CreateMap<ProjectDTO, ProjectGrpcDTO>().ReverseMap();

            CreateMap(typeof(ProjectPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            //CreateMap<ServicePageListGrpcDTO, IPagedList<ServiceDTO>>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
