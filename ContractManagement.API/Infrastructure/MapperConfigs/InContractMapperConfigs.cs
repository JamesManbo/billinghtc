using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.SharingRevenueModels;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class InContractMapperConfigs : Profile
    {
        public InContractMapperConfigs()
        {
            CreateMap<InContract, InContractDTO>();
            CreateMap<InContract, InContractGridDTO>();
            CreateMap<InContract, InContractSimpleDTO>();
            CreateMap<CreateInContractCommand, InContract>();

            //CreateMap<CUInContractServiceCommand, InContractService>();
            //CreateMap<InContractService, InContractServiceDTO>();
        }
    }
}