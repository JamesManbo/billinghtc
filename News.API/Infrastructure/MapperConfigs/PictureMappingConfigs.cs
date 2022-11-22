using AutoMapper;
using Picture.StaticResource.API.Proto;
using News.API.Models.Domain;

namespace News.API.Infrastructure.MapperConfigs
{
    public class PictureMappingConfigs : Profile
    {
        public PictureMappingConfigs()
        {
            CreateMap<PictureGrpcDto, Models.Picture>();
            CreateMap<Models.Picture, PictureViewModel>();
        }
    }
}
