using AutoMapper;
using Global.Models.Filter;
using OrganizationUnit.API.Protos.Configurations;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class CommonsMapperProfile : Profile
    {
        public CommonsMapperProfile()
        {
            CreateMap<RequestFilterModel, Protos.Organizations.RequestFilterGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, Protos.Users.RequestFilterGrpc>().ReverseMap();
            CreateMap<ConfigurationSystemParameterDto, SystemConfigurationParamsGrpcDTO>().ReverseMap();
        }
    }
}
