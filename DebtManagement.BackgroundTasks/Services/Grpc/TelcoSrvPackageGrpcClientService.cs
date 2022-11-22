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
    public interface ITelcoServicePackageGrpcService
    {
        Task<ListServicePackageGrpcResponse> GetAll();
    }
    public class TelcoSrvPackageGrpcClientService : GrpcCaller, ITelcoServicePackageGrpcService
    {
        public TelcoSrvPackageGrpcClientService(IMapper mapper, ILogger<TelcoServiceGrpcService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ListServicePackageGrpcResponse> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new TelcoServicePackageGrpcService.TelcoServicePackageGrpcServiceClient(channel);
                return await client.GetAllAsync(new Empty());
            });
        }
    }
}
