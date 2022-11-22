using AutoMapper;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class ClearingMapperConfigs : Profile
    {
        public ClearingMapperConfigs()
        {
            CreateMap<Clearing, ClearingDTO>().ReverseMap();
        }
    }
}
