using AutoMapper;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using News.API.Infrastructure.Queries;
using News.API.Models.RequestModels;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Grpc
{
    public class ArticleService : NewsGrpc.NewsGrpcBase
    {
        private readonly IArticlesQueries _articlesQueries;
        private readonly IArticleCategoryQueries _articleCategoryQueries;
        private readonly IMapper _mapper;
        public ArticleService(
            IArticlesQueries articlesQueries,
            IArticleCategoryQueries articleCategoryQueries, 
            IMapper mapper)
        {
            _articlesQueries = articlesQueries;
            _articleCategoryQueries = articleCategoryQueries;
            _mapper = mapper;
        }

        public override Task<ArticlePageListGrpcDTO> GetArticles(ArticleRequestFilterGrpc request, ServerCallContext context)
        {
            var lstArticle = _articlesQueries.GetList(_mapper.Map<ArticleFilterModel>(request));
            return Task.FromResult(_mapper.Map<ArticlePageListGrpcDTO>(lstArticle));
        }

        public override Task<ArticleTypeListGrpcDTO> GetArticleTypes(Empty request, ServerCallContext context)
        {
            var lstArticleType = _articleCategoryQueries.GetAll()?.ToList();
            var result = new ArticleTypeListGrpcDTO();
            if (lstArticleType != null && lstArticleType.Any())
            {
                for (int i = 0; i < lstArticleType.Count; i++)
                {
                    result.Subset.Add(_mapper.Map<ArticleTypeDTOGrpc>(lstArticleType.ElementAt(i)));
                }
            }

            return Task.FromResult(result);
        }
    }
}
