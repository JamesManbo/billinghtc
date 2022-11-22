using AutoMapper;
using Feedback.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface ISupportService
    {
        Task<IPagedList<SupportDTO>> GetList(RequestSupportFilterModel filterModel);
        Task<bool> CreateSupportRequest(SupportCommand supportRequest);
    }
    public class SupportService : GrpcCaller, ISupportService
    {
        private readonly IMapper _mapper;
        public SupportService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcFeedback)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateSupportRequest(SupportCommand supportRequest)
        {
            return await Call<bool>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<CreateFeedbackAndRequestGrpc>(supportRequest);
                var lstSupportGrpc = await client.CreateFeedbackAndRequestAsync(request);

                return lstSupportGrpc.Value;
            });
        }

        public async Task<IPagedList<SupportDTO>> GetList(RequestSupportFilterModel filterModel)
        {
            return await Call<IPagedList<SupportDTO>>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<FeedbackAndRequestFilterGrpc>(filterModel);
                //request.Source = "Application";
                var lstSupportGrpc = await client.GetFeedbackAndRequestsAsync(request);

                return _mapper.Map<IPagedList<SupportDTO>>(lstSupportGrpc);
            });
        }
    }
}
