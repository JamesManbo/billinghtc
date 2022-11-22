using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Clients
{
    public interface ITaxCategoryGrpcService
    {
        Task<IEnumerable<TaxCategoryDTO>> GetAll();
    }

    public class TaxCategoryGrpcService : GrpcCaller, ITaxCategoryGrpcService
    {
        private readonly IMapper _mapper;
        public TaxCategoryGrpcService(ILogger<GrpcCaller> logger, IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TaxCategoryDTO>> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var grpcClient = new TaxCategoryServiceGrpc.TaxCategoryServiceGrpcClient(channel);
                var grpcResponse = await grpcClient.GetAllAsync(new Google.Protobuf.WellKnownTypes.Empty());
                return grpcResponse.TaxCategoryDtos.Select(this._mapper.Map<TaxCategoryDTO>);
            });
        }
    }
}
