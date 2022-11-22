using AutoMapper;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Models.SupportModels;
using Feedback.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface ISupportService
    {
        Task<bool> CreateSupportRequest(SupportRequest supportRequest);
        Task<IPagedList<SupportDTO>> GetListByIdentityGuid(SupportRequestFilterModel filterModel);
        Task<SupportDTO> GetById(string id);
        Task<bool> CustomerRate(SupportRequest model);
    }
    public class SupportService : GrpcCaller, ISupportService
    {
        private readonly IMapper _mapper;
        public SupportService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcFeedback)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateSupportRequest(SupportRequest supportRequest)
        {
            return await Call<bool>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<CreateFeedbackAndRequestGrpc>(supportRequest);
                var lstSupportGrpc = await client.CreateFeedbackAndRequestAsync(request);

                return lstSupportGrpc.Value;
            });
        }

        public async Task<SupportDTO> GetById(string id)
        {
            return await Call<SupportDTO>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var supportGrpc = await client.GetByIdAsync(new StringValue {Value = id});

                return _mapper.Map<SupportDTO>(supportGrpc);
            });
        }

        public async Task<IPagedList<SupportDTO>> GetListByIdentityGuid(SupportRequestFilterModel filterModel)
        {
            return await Call<IPagedList<SupportDTO>>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<FeedbackAndRequestFilterGrpc>(filterModel);

                var lstSupportGrpc = await client.GetFeedbackAndRequestsAsync(request);

                return _mapper.Map<IPagedList<SupportDTO>>(lstSupportGrpc);
            });
        }

        public async Task<bool> CustomerRate(SupportRequest model)
        {
            return await Call<bool>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<CreateFeedbackAndRequestGrpc>(model);

                var lstSupportGrpc = await client.CustomerRateAsync(request);

                return lstSupportGrpc.Value;
            });
        }
    }
}
