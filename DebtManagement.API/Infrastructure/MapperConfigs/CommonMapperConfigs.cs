using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.CommonModels;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class CommonMapperConfigs : Profile
    {
        public CommonMapperConfigs()
        {
            CreateMap<ExchangeRateDTOGrpc, ExchangeRateDTO>();
        }
    }
}
