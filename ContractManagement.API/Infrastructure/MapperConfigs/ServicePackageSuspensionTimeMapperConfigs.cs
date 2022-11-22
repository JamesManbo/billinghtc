using AutoMapper;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models.OutContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ServicePackageSuspensionTimeMapperConfigs : Profile
    {
        public ServicePackageSuspensionTimeMapperConfigs()
        {
            CreateMap<ServicePackageSuspensionTime, ServicePackageSuspensionTimeDTO>().ReverseMap();
        }
    }
}
