using AutoMapper;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Domain.Commands.IntegrationEventCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class IntegrationEventMapperConfigs : Profile
    {
        public IntegrationEventMapperConfigs()
        {
            CreateMap<TerminateServicePackagesIntegrationEvent, TerminateServicePackagesIntegrationEventCommand>().ReverseMap();
            //CreateMap<UpgradeServicePackageIntegrationEvent, UpgradeServicePackageIntegrationEventCommand>().ReverseMap();
        }
    }
}
