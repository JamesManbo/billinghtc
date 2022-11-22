using ApplicationUserIdentity.API.Models.LocationDomainModels;
using AutoMapper;
using Location.API.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class LocationMappingConfigs : Profile
    {
        public LocationMappingConfigs()
        {
            CreateMap<LocationDTO, LocationGrpcDTO>().ReverseMap();
        }
    }
}
