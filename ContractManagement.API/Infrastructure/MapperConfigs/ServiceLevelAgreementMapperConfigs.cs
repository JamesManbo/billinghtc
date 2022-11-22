using AutoMapper;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Models;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ServiceLevelAgreementMapperConfigs : Profile
    {
        public ServiceLevelAgreementMapperConfigs()
        {
            CreateMap<ServiceLevelAgreement, ServiceLevelAgreementDTO>();
            CreateMap<CUServiceLevelAgreementCommand, ServiceLevelAgreement>();
        }
    }
}
