using AutoMapper;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Models;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContactInfoMapperConfigs : Profile
    {
        public ContactInfoMapperConfigs()
        {
            CreateMap<ContactInfo, ContactInfoDTO>().ReverseMap();

            CreateMap<CUContactInfoCommand, ContactInfo>();
        }
    }
}
