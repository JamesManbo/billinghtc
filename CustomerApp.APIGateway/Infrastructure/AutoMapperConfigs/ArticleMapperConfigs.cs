using AutoMapper;
using CustomerApp.APIGateway.Models.ArticleModels;
using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Models.PagedList;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ArticleMapperConfigs : Profile
    {
        public ArticleMapperConfigs()
        {
            CreateMap<ArticleFilterModel, ArticleRequestFilterGrpc>().ReverseMap();
            CreateMap<ArticleDTO, ArticleDTOGrpc>().ReverseMap();
            CreateMap<PictureDTOGrpc, PictureViewModel>().ReverseMap();
            CreateMap(typeof(ArticlePageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

        }
    }
}
