using AutoMapper;
using Global.Models.Filter;
using Global.Models.PagedList;
using News.API.Models;
using News.API.Models.Domain;
using News.API.Models.RequestModels;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.MapperConfigs
{
    public class ArticleMappingConfigs : Profile
    {
        public ArticleMappingConfigs()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();

            CreateMap<ArticleDto, ArticleDTOGrpc>().ReverseMap();
            CreateMap<ArticleFilterModel, ArticleRequestFilterGrpc>().ReverseMap();
            CreateMap<IPagedList<ArticleDto>, ArticlePageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
