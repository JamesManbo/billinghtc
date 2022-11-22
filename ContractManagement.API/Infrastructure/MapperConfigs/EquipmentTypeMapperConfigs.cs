using AutoMapper;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Models;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.EquipmentTypeCommand;
using ContractManagement.API.Protos;
using Global.Models.PagedList;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class EquipmentTypeMapperConfigs : Profile
    {
        public EquipmentTypeMapperConfigs()
        {
            CreateMap<EquipmentType, EquipmentTypeDTO>().ReverseMap();
            CreateMap<EquipmentType, CreateEquipmentTypeCommand>().ReverseMap();
            CreateMap<EquipmentType, UpdateEquipmentTypeCommand>().ReverseMap();
            CreateMap<CreateEquipmentTypeCommand, EquipmentTypeDTO>().ReverseMap();
            CreateMap<UpdateEquipmentTypeCommand, EquipmentTypeDTO>().ReverseMap();
            CreateMap<EquipmentType, SelectionItem>().ReverseMap();

            CreateMap<EquipmentTypeDTO, EquipmentTypeGrpcDTO>();
            CreateMap<IPagedList<EquipmentTypeDTO>, EquipmentTypePageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
