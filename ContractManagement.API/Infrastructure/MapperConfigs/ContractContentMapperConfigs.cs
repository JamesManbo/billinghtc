using AutoMapper;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.ContractContentCommand;
using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractContentMapperConfigs : Profile
    {
        public ContractContentMapperConfigs()
        {
            CreateMap<CUContractContentCommand, ContractContent>()
                .ForMember(e => e.DigitalSignature, m => m.Ignore())
                .ForMember(e => e.ContractFormSignature, m => m.Ignore());
            CreateMap<ContractContent, ContractContentDTO>().ReverseMap();
        }
    }
}
