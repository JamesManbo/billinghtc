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
    public interface ITelcoServiceGrpcService
    {
        Task<ServiceListGrpcDTO> GetAll();
    }
    public class TelcoServiceGrpcService : GrpcCaller, ITelcoServiceGrpcService
    {
        public TelcoServiceGrpcService(IMapper mapper, ILogger<TelcoServiceGrpcService> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
        }

        public async Task<ServiceListGrpcDTO> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new ServiceGrpc.ServiceGrpcClient(channel);
                return await client.GetAllServiceAsync(new Empty());
            });
        }
    }
}
