using AutoMapper;
using ContractManagement.Domain.Models;
using Picture.StaticResource.API.Proto;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class PictureMappingConfigs : Profile
    {
        public PictureMappingConfigs()
        {
            CreateMap<ContractManagement.Domain.AggregateModels.PictureAggregate.Picture, PictureDTO>().ReverseMap();
            CreateMap<ContractManagement.Domain.AggregateModels.PictureAggregate.Picture, PictureGrpcDto>().ReverseMap();
            //CreateMap<PictureGrpcDto, Picture>().ReverseMap();
        }
    }
}
