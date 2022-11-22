using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface IExchangeRateGrpcService
    {
        Task<List<ExchangeRateDTO>> GetExchangeRatesByNow();
        Task<bool> SynchronizeExchangeRates();
    }
    public class ExchangeRateGrpcService : GrpcCaller, IExchangeRateGrpcService
    {
        public ExchangeRateGrpcService(IMapper mapper, ILogger<GrpcCaller> logger)
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
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

    public class ExchangeRateDTO
    {
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Buy { get; set; }
        public string Transfer { get; set; }
        public string Sell { get; set; }
        public double BuyValue { get; set; }
        public double TransferValue { get; set; }
        public double SellValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
