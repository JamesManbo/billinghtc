using AutoMapper;
using ContractManagement.Domain.Models.Location;
using Location.API.Proto;

namespace ContractManagement.API.Infrastructure.MapperConfigs.Location
{
    public class LocationMapperConfigs: Profile
    {
        public LocationMapperConfigs()
        {
            CreateMap<LocationDTO, LocationGrpcDTO>().ReverseMap();
        }
    }
}
