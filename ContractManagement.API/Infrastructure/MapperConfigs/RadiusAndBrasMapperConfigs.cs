using AutoMapper;
using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Models.RadiusAndBras;
using ContractManagement.RadiusDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class RadiusAndBrasMapperConfigs : Profile
    {
        public RadiusAndBrasMapperConfigs()
        {
            CreateMap<RadiusServerInformation, RadiusServerInfoDTO>();
            CreateMap<CuRadiusServerInfoCommand, RadiusServerInformation>();

            CreateMap<BrasInformation, BrasInfoDTO>();
            CreateMap<CuBrasInfoCommand, BrasInformation>();

            CreateMap<RmServices, RadiusServiceDTO>();
        }
    }
}
