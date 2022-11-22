using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ExchangeRateMapperConfigs : Profile
    {
        public ExchangeRateMapperConfigs()
        {
            CreateMap<ExchangeRateDTOGrpc, ExchangeRateDTO>().ReverseMap();
        }
    }
}
