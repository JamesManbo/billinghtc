using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class TransactionEquipmentMapperConfig : Profile
    {
        public TransactionEquipmentMapperConfig()
        {
            CreateMap<TransactionEquipment, TransactionEquipmentDTO>();
            CreateMap<CUTransactionEquipmentCommand, TransactionEquipment>();

            CreateMap<TransactionEquipmentDTO, CUContractEquipmentCommand>().ReverseMap();
            CreateMap<TransactionEquipment, CUContractEquipmentCommand>().ReverseMap();
            CreateMap<ContractEquipment, CUContractEquipmentCommand>().ReverseMap();
            CreateMap<CUContractEquipmentCommand, CUTransactionEquipmentCommand>()
                .ForMember(s => s.ContractEquipmentId, m => m.MapFrom(d => d.Id))
                .ReverseMap();
            CreateMap<ContractEquipment, CUTransactionEquipmentCommand>()
                .ForMember(s => s.ContractEquipmentId, m => m.MapFrom(d => d.Id))
                .ReverseMap();

            CreateMap<TransactionEquipmentDTO, TransactionEquipmentDTOGrpc>().ReverseMap();
        }
    }
}
