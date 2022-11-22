using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.BackgroundTasks.Infrastructure.MapperConfigs
{
    public class ExchangeRateMapperConfigs : Profile
    {
        public ExchangeRateMapperConfigs()
        {
            CreateMap<ExchangeRateModel, ExchangeRate>().ReverseMap();
        }
    }
}
