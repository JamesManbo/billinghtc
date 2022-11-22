using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractorPropertiesMapperConfig : Profile
    {
        public ContractorPropertiesMapperConfig()
        {
            CreateMap<ContractorProperties, ContractorPropertiesDTO>().ReverseMap();
            CreateMap<CUContractorPropertiesCommand, ContractorPropertiesDTO>().ReverseMap();
            CreateMap<ContractorProperties, CUContractorPropertiesCommand>().ReverseMap();
        }
    }
}
