using AutoMapper;
using Global.Models.Filter;
using News.API.Models.Domain;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.MapperConfigs
{
    public class CommonMappingConfigs : Profile
    {
        public CommonMappingConfigs()
        {
            
            CreateMap<PictureViewModel, PictureDTOGrpc>().ReverseMap();
        }
    }
}
