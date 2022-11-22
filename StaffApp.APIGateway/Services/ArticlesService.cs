using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using News.API.Protos;
using StaffApp.APIGateway.Models.ArticleModels;
using StaffApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IArticlesService
    {
        Task<IPagedList<ArticleDTO>> GetList(ArticleFilterModel filterModel);
        Task<IEnumerable<ArticleTypeDTO>> GetListArticleType();
    }
    public class ArticlesService: GrpcCaller, IArticlesService
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

        public async Task<IEnumerable<ArticleTypeDTO>> GetListArticleType()
        {
            return await Call<IEnumerable<ArticleTypeDTO>>(async channel =>
            {
                var client = new NewsGrpc.NewsGrpcClient(channel);

                var lstArticleGrpc = await client.GetArticleTypesAsync(new Empty());

                return _mapper.Map<IEnumerable<ArticleTypeDTO>>(lstArticleGrpc.Subset);
            });
        }
    }
}
