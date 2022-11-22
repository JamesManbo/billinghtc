using AutoMapper;
using OrganizationUnit.API.Application.Commands.ConfigurationSettingUser;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class ConfigurationSettingUser : Profile
    {
        public ConfigurationSettingUser()
        {
            CreateMap<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount, CUConfigurationSettingUserCommand>().ReverseMap();
            CreateMap<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount, ConfigurationSettingUserDto>().ReverseMap();
            CreateMap<CUConfigurationSettingUserCommand, ConfigurationSettingUserDto>().ReverseMap();
        }
    }

    public class ConfigurationParameterSystemUser : Profile
    {
        public ConfigurationParameterSystemUser()
        {
            CreateMap<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter, CUConfigurationSystemParameterCommand>().ReverseMap();
            CreateMap<Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationSystemParameter, ConfigurationSystemParameterDto>().ReverseMap();
            CreateMap<CUConfigurationSystemParameterCommand, ConfigurationSystemParameterDto>().ReverseMap();
        }
    }
}
