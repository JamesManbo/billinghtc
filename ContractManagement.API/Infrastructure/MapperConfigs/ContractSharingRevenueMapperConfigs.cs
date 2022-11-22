using AutoMapper;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.SharingRevenueModels;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractSharingRevenueMapperConfigs : Profile
    {
        public ContractSharingRevenueMapperConfigs()
        {
            CreateMap<ContractSharingRevenueLine, ContractSharingRevenueLineDTO>().ReverseMap();

            CreateMap<CUContractSharingRevenueLineCommand, ContractSharingRevenueLine>().ReverseMap();
            CreateMap<ContractSharingRevenueLine, ContractSharingRevenueLineInsertBulkModel>().ReverseMap();
        }
    }
}
