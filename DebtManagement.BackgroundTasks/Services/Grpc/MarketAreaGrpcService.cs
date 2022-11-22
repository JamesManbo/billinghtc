using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface IMarketAreaGrpcService
    {
        Task<MarketAreaListGrpcDTO> GetAll();
    }
    public class MarketAreaGrpcService : GrpcCaller, IMarketAreaGrpcService
    {
        public MarketAreaGrpcService(IMapper mapper, ILogger<MarketAreaGrpcService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<MarketAreaListGrpcDTO> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new MarketAreaServiceGrpc.MarketAreaServiceGrpcClient(channel);
                return await client.GetAllAsync(new Empty());
            });
        }
    }
}
