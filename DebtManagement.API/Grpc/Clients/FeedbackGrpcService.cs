using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.Domain.Commands.Commons;
using DebtManagement.Domain.Models.FeedbackModels;
using Feedback.API.Protos;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace DebtManagement.API.Grpc.Clients.StaticResource
{
    public interface IFeedbackGrpcService
    {
        Task<List<FeedbackAndRequestDTO>> GetUnhandledFeedbackByCIds(string[] cIds);
        Task<bool> MarkFeedbacksAsResolved(string feedbackIds);
        Task<IEnumerable<(string, int)>> CountFeedbackByCIds(string[] cIds);
        Task<bool> UpdateReceiptLine(string ids, int receiptLineId);
    }

    public class FeedbackGrpcService : GrpcCaller, IFeedbackGrpcService
    {
        private readonly IMapper _mapper;
        public FeedbackGrpcService(ILogger<GrpcCaller> logger,
            IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcFeedback)
        {
            this._mapper = mapper;
        }

        public async Task<bool> MarkFeedbacksAsResolved(string feedbackIds)
        {
            return await CallAsync(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var result = await client.MarkFeedbacksAsResolvedAsync(new StringValue()
                {
                    Value = feedbackIds
                });

                return result.Value;
            });
        }

        public async Task<IEnumerable<(string, int)>> CountFeedbackByCIds(string[] cIds)
        {
            return await CallAsync<IEnumerable<(string, int)>>(async channel =>
            {
                var result = new List<(string, int)>();
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var response = await client.CountFeedbackByCIdsAsync(new StringValue()
                {
                    Value = string.Join(',', cIds)
                });

                foreach (var item in response.CountingItems)
                {
                    result.Add((item.CId, item.Count ?? 0));
                }

                return result;
            });
        }

        public async Task<List<FeedbackAndRequestDTO>> GetUnhandledFeedbackByCIds(string[] cIds)
        {
            return await CallAsync(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var result = await client.GetUnresolvedFeedbacksByCIdsAsync(new StringValue()
                {
                    Value = string.Join(',', cIds)
                });

                return this._mapper.Map<List<FeedbackAndRequestDTO>>(result.Result);
            });
        }

        public async Task<bool> UpdateReceiptLine(string ids, int receiptLineId)
        {
            return await CallAsync(async channel =>
            {
                var client = new FeedbackAndRequestGrpc.FeedbackAndRequestGrpcClient(channel);
                var result = await client.UpdateReceiptLineIdAsync(new UpdateReceiptLineRequestGrpc()
                {
                    Ids = ids,
                    ReceiptLineId = receiptLineId
                });

                return result.Value;
            });
        }
    }
}
