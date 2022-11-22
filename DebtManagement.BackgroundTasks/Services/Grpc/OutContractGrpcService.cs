using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.BackgroundTasks.Models.Contracts;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface IOutContractGrpcService
    {
        Task<IEnumerable<PromotionDTO>> GetAvailablePromotions();
    }

    public class OutContractGrpcService : GrpcCaller,
        IOutContractGrpcService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcCaller> _logger;
        public OutContractGrpcService(IMapper mapper,
            ILogger<OutContractGrpcService> logger)
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<PromotionDTO>> GetAvailablePromotions()
        {
            return await CallAsync(async channel =>
            {
                var grpcClient = new ContractManagementGrpc.ContractManagementGrpcClient(channel);
                var grpcResponse = await grpcClient.GetAvailablePromotionsAsync(new Empty());
                if (grpcResponse != null && grpcResponse.PromotionGrpcModels.Count > 0)
                {
                    return grpcResponse
                        .PromotionGrpcModels
                        .Select(this._mapper.Map<PromotionDTO>);
                }

                return Enumerable.Empty<PromotionDTO>();
            });
        }
    }
}
