using AutoMapper;
using Feedback.API.Protos;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using Feedback.API.Models;
using Feedback.API.Repository;
using Google.Protobuf.WellKnownTypes;
using Feedback.API.Queries;

namespace Feedback.API.Grpc
{
    public class FeedbackAndRequestServiceGrpc : FeedbackAndRequestGrpc.FeedbackAndRequestGrpcBase
    {
        private readonly IFeedbackAndRequestRepository _feedbackAndRequestRepository;
        private readonly IFeedbackAndRequestQueries _feedbackAndRequestQueries;
        private readonly IMapper _mapper;
        public FeedbackAndRequestServiceGrpc(IFeedbackAndRequestRepository feedbackAndRequestRepository,
            IFeedbackAndRequestQueries feedbackAndRequestQueries,
            IMapper mapper)
        {
            _feedbackAndRequestRepository = feedbackAndRequestRepository;
            _feedbackAndRequestQueries = feedbackAndRequestQueries;
            _mapper = mapper;
        }

        public override async Task<FeedbackAndRequestPageListGrpcDTO> GetFeedbackAndRequests(FeedbackAndRequestFilterGrpc request, ServerCallContext context)
        {
            var feedbackAndRequests = await _feedbackAndRequestRepository.GetList(_mapper.Map<FeedbackAndRequestFilterModel>(request));
            return _mapper.Map<FeedbackAndRequestPageListGrpcDTO>(feedbackAndRequests);
        }

        public override async Task<BoolValue> CreateFeedbackAndRequest(CreateFeedbackAndRequestGrpc request, ServerCallContext context)
        {
            var actionResponse = await _feedbackAndRequestRepository.Create(_mapper.Map<FeedbackAndRequest>(request));

            return new BoolValue
            {
                Value = actionResponse.IsSuccess
            };
        }
        public override async Task<BoolValue> UpdateFeedbackAndRequest(CreateFeedbackAndRequestGrpc request, ServerCallContext context)
        {
            var actionResponse = await _feedbackAndRequestRepository
                .Update(_mapper.Map<FeedbackAndRequest>(request));

            return new BoolValue
            {
                Value = actionResponse.IsSuccess
            };
        }

        public override async Task<BoolValue> CustomerRate(CreateFeedbackAndRequestGrpc request, ServerCallContext context)
        {
            var actionResponse = await _feedbackAndRequestRepository
                .Update(_mapper.Map<FeedbackAndRequest>(request));

            return new BoolValue
            {
                Value = actionResponse.IsSuccess
            };
        }

        public override async Task<FeedbackAndRequestAllGrpc> GetUnresolvedFeedbacksByCIds(StringValue request, ServerCallContext context)
        {
            var result = _feedbackAndRequestQueries.GetUnresolvedFeedbacksByCIds(request.Value);
            return _mapper.Map<FeedbackAndRequestAllGrpc>(result);
        }
        public override async Task<BoolValue> MarkFeedbacksAsResolved(StringValue request, ServerCallContext context)
        {
            var feedbackIds = request.Value.Split("##");
            var actionResponse = await _feedbackAndRequestRepository.MarkFeedbacksAsResolved(feedbackIds);
            return new BoolValue
            {
                Value = actionResponse.IsSuccess
            };
        }

        public override async Task<CountFeedbackByCIdGrpcResponse> CountFeedbackByCIds(StringValue request, ServerCallContext context)
        {
            var queryEnumerable = _feedbackAndRequestQueries.CountingFeedbacksByCId(request.Value);

            var result = new CountFeedbackByCIdGrpcResponse();
            foreach (var item in queryEnumerable)
            {
                var countingItem = new CountFeedbackItemGrpc()
                {
                    CId = item.Item1,
                    Count = item.Item2
                };
                result.CountingItems.Add(countingItem);
            }

            return result;
        }

        public override async Task<FeedbackAndRequestDTOGrpc> GetById(StringValue request, ServerCallContext context)
        {
            var result = await _feedbackAndRequestQueries.Get(request.Value);
            return _mapper.Map<FeedbackAndRequestDTOGrpc>(result);
        }

        public override async Task<BoolValue> UpdateReceiptLineId(UpdateReceiptLineRequestGrpc request, ServerCallContext context)
        {
            if (request != null && !string.IsNullOrEmpty(request.Ids))
            {
                var lstId = request.Ids.Split("##");
                var allSuccess = true;
                foreach (var id in lstId)
                {
                    var existModel = await _feedbackAndRequestQueries.Get(id);
                    if (existModel != null)
                    {
                        existModel.ReceiptLineId = request.ReceiptLineId.HasValue ? request.ReceiptLineId.Value : 0;
                        var actionResponse = await _feedbackAndRequestRepository.Update(existModel);
                        allSuccess = allSuccess && actionResponse.IsSuccess;
                    }
                }
                return new BoolValue
                {
                    Value = allSuccess
                };
            }


            return new BoolValue
            {
                Value = false
            };
        }
    }
}
