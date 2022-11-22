using AutoMapper;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.SharingRevenueModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractSharingRevenueLineMapperConfigs : Profile
    {
        public ContractSharingRevenueLineMapperConfigs()
        {
            CreateMap<ContractSharingRevenueLine, ContractSharingRevenueLineDTO>().ReverseMap();

            CreateMap<CUContractSharingRevenueLineCommand, ContractSharingRevenueLine>().ReverseMap();
            CreateMap<ContractSharingRevenueLine, ContractSharingRevenueLineInsertBulkModel>().ReverseMap();
        }
    }
}
