using AutoMapper;
using News.API.Models;
using News.API.Models.Domain;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.MapperConfigs
{
    public class ArticleCategoryMappingConfig : Profile
    {
        public ArticleCategoryMappingConfig()
        {
            CreateMap<ArticleCategory, ArticleCategoryDto>().ReverseMap();
            CreateMap<ArticleCategoryDto, ArticleTypeDTOGrpc>().ReverseMap();
        }
    }
}
