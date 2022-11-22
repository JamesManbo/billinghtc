using AutoMapper;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.EquipmentCommand;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class EquipmentPictureMapperConfigs : Profile
    {
        public EquipmentPictureMapperConfigs()
        {
            CreateMap<EquipmentPicture, EquipmentPictureDTO>().ReverseMap();
            CreateMap<CUEquipmentPictureCommand, EquipmentPictureDTO>().ReverseMap();
            CreateMap<EquipmentPicture, CUEquipmentPictureCommand>().ReverseMap();
        }
    }
}
