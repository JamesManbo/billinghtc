using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public interface ITaxCategoryGrpcClientService
    {
        Task<List<TaxCategoryDTO>> GetAll();
    }
    public class TaxCategoryGrpcClientService : GrpcCaller, ITaxCategoryGrpcClientService
    {
        private readonly IMapper _mapper;

        public TaxCategoryGrpcClientService(IMapper mapper, ILogger<GrpcCaller> logger) 
            : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<List<TaxCategoryDTO>> GetAll()
        {
            return await CallAsync(async channel =>
            {
                var client = new TaxCategoryServiceGrpc.TaxCategoryServiceGrpcClient(channel);
                var grpcResponse = await client.GetAllAsync(new Empty());               
                return grpcResponse?.TaxCategoryDtos
                    .Select(_mapper.Map<TaxCategoryDTO>)
                    .ToList(); 
            });
        }
    }
}
