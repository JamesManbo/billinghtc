using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.ExchangeRateAggregate;
using ContractManagement.Domain.Commands.CUCurrencyUnitCommand;
using ContractManagement.Domain.Models;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class CurrencyUnitMapperConfigs : Profile
    {
        public CurrencyUnitMapperConfigs()
        {
            CreateMap<CurrencyUnit, CurrencyUnitDTO>().ReverseMap();
            CreateMap<CurrencyUnit, CreateCurrencyUnitCommand>().ReverseMap();
            CreateMap<CurrencyUnit, UpdateCurrencyUnitCommand>().ReverseMap();
            CreateMap<CreateCurrencyUnitCommand, EquipmentTypeDTO>().ReverseMap();
            CreateMap<UpdateCurrencyUnitCommand, EquipmentTypeDTO>().ReverseMap();
            CreateMap<CurrencyUnit, SelectionItem>().ReverseMap();
            CreateMap<ExchangeRateModel, ExchangeRateDTOGrpc>().ReverseMap();
            CreateMap<ExchangeRateModel, ExchangeRate>().ReverseMap();
        }
    }
}
