using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using Picture.StaticResource.API.Proto;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class PictureMappingConfigs : Profile
    {
        public PictureMappingConfigs()
        {
            CreateMap<Models.Picture, PictureViewModel>().ReverseMap();
            CreateMap<PictureGrpcDto, Models.Picture>().ReverseMap();
            CreateMap<PictureDtoGrpc, PictureViewModel>().ReverseMap();
        }
    }
}
