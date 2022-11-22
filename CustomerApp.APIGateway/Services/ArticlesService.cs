using AutoMapper;
using CustomerApp.APIGateway.Models.ArticleModels;
using CustomerApp.APIGateway.Models.RequestModels;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using News.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IArticlesService
    {
        Task<IPagedList<ArticleDTO>> GetList(ArticleFilterModel filterModel);
    }
    public class ArticlesService : GrpcCaller, IArticlesService
    {
        private readonly IMapper _mapper;
        public ArticlesService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcNews)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ArticleDTO>> GetList(ArticleFilterModel filterModel)
        {
            return await Call<IPagedList<ArticleDTO>>(async channel =>
            {
                var client = new NewsGrpc.NewsGrpcClient(channel);
                var request = _mapper.Map<ArticleRequestFilterGrpc>(filterModel);

                var lstArticleGrpc = await client.GetArticlesAsync(request);

                return _mapper.Map<IPagedList<ArticleDTO>>(lstArticleGrpc);
            });
        }
    }
}
