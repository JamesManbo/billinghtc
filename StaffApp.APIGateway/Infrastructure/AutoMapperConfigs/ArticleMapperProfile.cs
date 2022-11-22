using AutoMapper;
using Global.Models.Filter;
using Global.Models.PagedList;
using News.API.Protos;
using StaffApp.APIGateway.Models.ArticleModels;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ArticleMapperProfile : Profile
    {
        public ArticleMapperProfile()
        {

            CreateMap<ArticleDTO, ArticleDTOGrpc>().ReverseMap();
            CreateMap<PictureDTOGrpc, PictureViewDTO>().ReverseMap();
            CreateMap<ArticleFilterModel, ArticleRequestFilterGrpc>().ReverseMap();

            CreateMap(typeof(ArticlePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<ArticleTypeDTO, ArticleTypeDTOGrpc>().ReverseMap();
        }
    }
}
