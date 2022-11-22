using AutoMapper;
using ContractManagement.API.Application.Commands.UnitOfMeasurementCommand;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class UnitOfMeasurementMapperConfig : Profile
    {
        public UnitOfMeasurementMapperConfig()
        {
            CreateMap<UnitOfMeasurement, UnitOfMeasurementDTO>().ReverseMap();
            CreateMap<UnitOfMeasurement, CUUnitOfMeasurementCommand>().ReverseMap();
            CreateMap<CUUnitOfMeasurementCommand, UnitOfMeasurementDTO>().ReverseMap();

            CreateMap<UnitOfMeasurementFilterModel, UnitOfMeasurementFilterGrpc>().ReverseMap();
        }
    }
}
