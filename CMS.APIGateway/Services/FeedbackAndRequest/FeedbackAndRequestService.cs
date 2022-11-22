using AutoMapper;
using CMS.APIGateway.Models.FeedbackAndRequest;
using Feedback.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Global.Models.StateChangedResponse;

namespace CMS.APIGateway.Services.FeedbackAndRequest
{
    public interface IFeedbackAndRequestService
    {
        Task<bool> CreateFeedbackOrRequest(CUFeedbackAndRequest applicationUser);
        Task<IPagedList<FeedbackAndRequestDtoGrpc>> GetList(FeedbackAndRequestFilterModel filterModel);
        Task<bool> UpdateFeedbackOrRequest(CUFeedbackAndRequest cUFeedback);
    }
    public class FeedbackAndRequestService : GrpcCaller, IFeedbackAndRequestService
    {
        private IMapper _mapper;
        public FeedbackAndRequestService(IMapper mapper, ILogger<FeedbackAndRequestService> logger) 
            : base(mapper,logger,  MicroserviceRouterConfig.GrpcFeedback)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateFeedbackOrRequest(CUFeedbackAndRequest fbCreateCommand)
        {   
            return await Call<bool>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<CreateFeedbackAndRequestGrpc>(fbCreateCommand);
                //request.Source = "HTC_ITC_Ticket_System";

                return await client.CreateFeedbackAndRequestAsync(request);
            });
        }

        public async Task<IPagedList<FeedbackAndRequestDtoGrpc>> GetList(FeedbackAndRequestFilterModel filterModel)
        {
            return await Call<IPagedList<FeedbackAndRequestDtoGrpc>>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<FeedbackAndRequestFilterGrpc>(filterModel);
                //request.Source = "CMS";
                var lstSupportGrpc = await client.GetFeedbackAndRequestsAsync(request);

                return _mapper.Map<IPagedList<FeedbackAndRequestDtoGrpc>>(lstSupportGrpc);
            });
        }

        public async Task<bool> UpdateFeedbackOrRequest(CUFeedbackAndRequest cUFeedback)
        {
            return await Call<bool>(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var request = _mapper.Map<CreateFeedbackAndRequestGrpc>(cUFeedback);
                request.Source = "HTC_ITC_Ticket_System";
                return await client.UpdateFeedbackAndRequestAsync(request);
            });
        }
    }
}
