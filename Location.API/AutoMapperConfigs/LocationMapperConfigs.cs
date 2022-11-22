using AutoMapper;
using Location.API.Commands;
using Location.API.Model;
using Location.API.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.AutoMapperConfigs
{
    public class LocationMapperConfigs: Profile
    {
        public LocationMapperConfigs()
        {
            CreateMap<LocationDTO, LocationGrpcDTO>().ReverseMap();
            CreateMap<LocationDTO, Model.LocationModel>().ReverseMap();
            CreateMap<CULocationCommand, Model.LocationModel>().ReverseMap();
            CreateMap<CULocationCommand, CreateLocationGrpcCommand>().ReverseMap();
            CreateMap<LocationModel, LocationSelectionItem>()
                .ForMember(t => t.Value, f => f.MapFrom(m => m.LocationId))
                .ForMember(t => t.Text, f => f.MapFrom(m => m.Name));
            CreateMap<LocationModel, LocationGrpcDTO>().ReverseMap();
        }
    }
}
