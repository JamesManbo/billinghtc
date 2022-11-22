using ContractManagement.API.Protos;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface IMarketAreaGrpcService
    {
        Task<string> GetMarketAreaCode(int id);
    }
    public class MarketAreaGrpcService : GrpcCaller, IMarketAreaGrpcService
    {
        public MarketAreaGrpcService(ILogger<GrpcCaller> logger)
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<string> GetMarketAreaCode(int id)
        {
            return await CallAsync(async channel =>
            {
                var client = new MarketAreaServiceGrpc.MarketAreaServiceGrpcClient(channel);
                var result = await client.GetMarketAreaCodeAsync(new Int32Value { Value = id });
                return result.Value;
            });
        }
    }
}
