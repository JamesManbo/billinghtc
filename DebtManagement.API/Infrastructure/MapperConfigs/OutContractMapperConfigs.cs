using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.ContractModels;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class OutContractMapperConfigs : Profile
    {
        public OutContractMapperConfigs()
        {
            CreateMap<OutContractSimpleGrpcDTO, OutContractDTO>().ReverseMap();
        }
    }
}
