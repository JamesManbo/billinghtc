using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Picture.StaticResource.API.Proto;
using StaticResource.API.Models;

namespace StaticResource.API.Infrastructure.AutomapperConfigs
{
    public class PictureMappingProfile : Profile
    {
        public PictureMappingProfile()
        {
            CreateMap<PictureModel, PictureGrpcDto>().ReverseMap();
        }
    }
}
