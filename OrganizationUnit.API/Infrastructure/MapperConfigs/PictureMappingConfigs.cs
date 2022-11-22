using AutoMapper;
using OrganizationUnit.Domain.AggregateModels.PictureAggregate;
using OrganizationUnit.Domain.Models.Common;
using Picture.StaticResource.API.Proto;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class PictureMappingConfigs : Profile
    {
        public PictureMappingConfigs()
        {
            CreateMap<PictureDto, Domain.AggregateModels.PictureAggregate.Picture>().ReverseMap();
            CreateMap<PictureGrpcDto, Domain.AggregateModels.PictureAggregate.Picture>().ReverseMap();
        }
    }
}
