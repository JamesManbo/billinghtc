using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.CommonModels;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IExchangeRateGrpcService
    {
        Task<ExchangeRateGrpcResult> ExchangeRate(string fromCode, string toCode);

        Task<List<ExchangeRateDTO>> GetExchangeRatesByNow();
        Task<bool> SynchronizeExchangeRates();
    }
    public class ExchangeRateGrpcService : GrpcCaller, IExchangeRateGrpcService
    {
        private readonly IMapper _mapper;
        public ExchangeRateGrpcService(ILogger<GrpcCaller> logger,
            IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
            this._mapper = mapper;
        }

        public async Task<ExchangeRateGrpcResult> ExchangeRate(string fromCode, string toCode)
        {
            return await CallAsync(async channel =>
            {
                var client = new ExchangeRateGrpc.ExchangeRateGrpcClient(channel);

                var transactionGrpc = await client.ExchangeRateAsync(new RequestExchangeRateGrpc { FromCode = fromCode, ToCode = toCode});
                return transactionGrpc;
            });
        }
        public async Task<List<ExchangeRateDTO>> GetExchangeRatesByNow()
        {
            return await CallAsync<List<ExchangeRateDTO>>(async channel =>
            {
                var client = new ExchangeRateGrpc.ExchangeRateGrpcClient(channel);
                var response = await client.GetExchangeRatesAsync(new Google.Protobuf.WellKnownTypes.Empty());

                if (response != null && response.Subset.Count > 0)
                {
                    return response.Subset
                        .Select(_mapper.Map<ExchangeRateDTO>).ToList();
                }

                return default;
            });
        }

        public async Task<bool> SynchronizeExchangeRates()
        {
            return await CallAsync(async channel =>
            {
                var client = new ExchangeRateGrpc.ExchangeRateGrpcClient(channel);

                var response = await client.SynchronizeAsync(new Google.Protobuf.WellKnownTypes.Empty());

                return response?.Value ?? false;
            });
        }
    }
}
